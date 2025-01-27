using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Courses.FileStorage
{
    /// <summary>
    /// Реализует интерфейс IFileStorage для файлового хранилища МиниО.
    /// </summary>
    public class MiniO : MinioClient, IFileStorage
    {
        private const char PathDelimeter = '/';
        private Cache<(byte[] data, string contentType)> Cache { get; init; }

        private string MainUrl { get; }
        private string SSLUrl { get; }

        public MiniO(IConfiguration config)
        {
            MainUrl = config["Storage:urlStorage"]!;
            SSLUrl = config["Storage:urlSSL"];
            if (!string.IsNullOrEmpty(SSLUrl))
            {
                MainUrl = SSLUrl;
            }

            //var urlStorage = config["Storage:urlStorage"];
            var accessKey = config["Storage:accessKey"];
            var secretKey = config["Storage:secretKey"];
            this.WithEndpoint(MainUrl)
                .WithCredentials(accessKey, secretKey);

            if (!string.IsNullOrEmpty(SSLUrl))
            {
                // TODO: Разобраться с со скачиванием по безопасным ссылкам 
                this.WithSSL();
            }
            this.Build();

            Cache = new(100);
        }

        /// <summary>
        /// Разделяет строку с fileName на кортеж строк
        /// (имя бакета, имя файла без префикса бакета)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns> Two strings tuple (bucketName, fileNameWithoutBucketPrefix) </returns>
        /// <exception cref="ArgumentException"></exception>
        private (string bucketName, string fileNameWithoutBucketPrefix) ParseFileName(string fileName)
        {
            if (!fileName.Contains(PathDelimeter))
            {
                throw new ArgumentException(
                    $"Имя файла {fileName} не содержит информации о директории хранения");
            }
            var tokens = fileName.Split(PathDelimeter);
            if (tokens.Length == 0)
            {
                throw new ArgumentException(
                    $"Имя файла {fileName} не содержит информации о директории хранения");
            }

            var bucketName = tokens[0];
            var fileNameWithoutBucketPrefix = fileName.Replace(bucketName + PathDelimeter, "");

            return (bucketName, fileNameWithoutBucketPrefix);
        }

        /// <summary>
        /// Удаляет файл/директорию fileName
        /// из бакета bucketName
        /// </summary>
        /// <param name="bucketName"> Имя бакета </param>
        /// <param name="fileName"> Имя файла/директории внутри бакета </param>
        private async Task DeleteFile(string bucketName, string fileName)
        {
            var isBucketExist = await BucketExistAsync(bucketName);
            if (!isBucketExist)
            {
                throw new ArgumentException($"В хранилище нет бакета с таким именем: {bucketName}");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await RemoveObjectAsync(removeObjectArgs);
        }

        /// <summary>
        /// Возвращает true если файл с именем fileName есть в бакете 
        /// и false в противном случае
        /// </summary>
        /// <param name="bucketName"/>
        /// <param name="fileName"/>
        private async Task<bool> ObjectExistAsync(string bucketName, string fileName)
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            try
            {
                await StatObjectAsync(statObjectArgs);
                return true;
            }
            catch (MinioException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Возвращает true если бакет с именем
        /// bucketName найден в хранилище
        /// </summary>
        /// <param name="bucketName"/>
        private async Task<bool> BucketExistAsync(string bucketName)
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool bucketFound = await BucketExistsAsync(beArgs);

            return bucketFound;
        }

        public string GetFileLink(string destinationFileFullName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<bool> FileExistAsync(string destinationFileFullName)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(destinationFileFullName);

            return await ObjectExistAsync(bucketName, fileNameWithoutBucketPrefix);
        }

        /// <inheritdoc/>
        public async Task UploadFileAsync(byte[] file, string destinationFileName)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(destinationFileName);

            // Создаём бакет, если его нет
            var isBucketExists = await BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!isBucketExists)
            {
                await MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            // Удаляем предыдущую версию файла
            await DeleteFile(bucketName, fileNameWithoutBucketPrefix);

            // Загружаем файл в бакет
            using var stream = new MemoryStream(file);
            await PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileNameWithoutBucketPrefix)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length));
        }

        /// <inheritdoc/>
        public async Task DeleteFileAsync(string filename)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(filename);

            await DeleteFile(bucketName, fileNameWithoutBucketPrefix);
        }

        public List<string> ListFiles(string destinationFolderFullName)
        {
            throw new NotImplementedException();
        }
    }
}