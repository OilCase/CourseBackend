using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace Courses.FileStorage
{
    /// <summary>
    /// Реализует интерфейс IFileStorage для файлового хранилища МиниО.
    /// </summary>
    public class MiniO : MinioClient, IFileStorage
    {
        private readonly char pathDelimeter = '/';
        private Cache<(byte[] data, string contentType)> Cache { get; init; }

        private string MainUrl { get; init; }
        private string SSLUrl { get; init; }

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
                // TODO: Разобраться с со скваичиванием по безопансным ссылкам 
                this.WithSSL();
            }
            this.Build();

            Cache = new(100);
        }

        public string GetFileLink(string destinationFileFullName)
        {
            throw new NotImplementedException();
        }

        public bool FileExist(string destinationFileFullName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task UploadFileAsync(byte[] file, string destinationFileName)
        {
            if (!destinationFileName.Contains(pathDelimeter))
            {
                throw new ArgumentException(
                    $"Имя файла {destinationFileName} не содержит информации о директории хранения");
            }
            var tokens = destinationFileName.Split('/');
            if (tokens.Length == 0)
            {
                throw new ArgumentException(
                    $"Имя файла {destinationFileName} не содержит информации о директории хранения");
            }

            var bucketName = tokens[0];
            var fileNameWithoutBucketPrefix = destinationFileName.Replace(bucketName + "/", "");

            // Создаём бакет, если его нет
            var bucketExists = await BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!bucketExists)
            {
                await MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            // Проверяем, существует ли файл и удаляем его, если существует
            if (IsObjectExist(bucketName, fileNameWithoutBucketPrefix))
            {
                await DeleteFile(bucketName, fileNameWithoutBucketPrefix);
            }

            // Загружаем файл в бакет
            using var stream = new MemoryStream(file);
            await PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileNameWithoutBucketPrefix)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length));
        }

        private async Task DeleteFile(string bucketName, string fileName)
        {
            if (!IsBucketExist(bucketName)) throw new ArgumentException($"В хранилище нет бакета с таким именем: {bucketName}");
            if (!IsObjectExist(bucketName, fileName)) throw new ArgumentException($"В бакете {bucketName} нет файла {fileName}");
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await RemoveObjectAsync(removeObjectArgs);
        }

        /// <summary>
        /// Удаляет файл из файлового хранилища МиниО
        /// </summary>
        /// <param name="filename">Строка с именем файла в формате "имя бакета/имя файла(можно с префиксами)"</param>
        public void DeleteFile(string filename)
        {
            var splittedFileName = filename.Split(new[] { '/' }, 2);
            var bucket = splittedFileName[0];
            var fileName = splittedFileName[1];

            if (!IsBucketExist(bucket)) throw new Exception($"В хранилище нет бакета с таким именем: {bucket}");

            if (!IsObjectExist(bucket, fileName)) throw new Exception($"В бакете {bucket} нет файла {fileName}");

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName);

            RemoveObjectAsync(removeObjectArgs).Wait();
        }

        public List<string> ListFiles(string destinationFolderFullName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает true если файл с именем fileName есть в бакете 
        /// и false в противном случае
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsObjectExist(string bucket, string fileName)
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName);

            try
            {
                StatObjectAsync(statObjectArgs).Wait();
                return true;
            }
            catch (Exception e)
            {
                var x = e.Message;
                return false;
            }
        }

        public bool IsBucketExist(string bucketName)
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool bucketFound = BucketExistsAsync(beArgs).Result;

            return bucketFound;
        }
    }
}