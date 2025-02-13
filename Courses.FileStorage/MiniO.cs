using System.IO.MemoryMappedFiles;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Courses.FileStorage
{
    /// <summary>
    /// Реализует интерфейс IFileStorage для файлового хранилища МиниО.
    /// </summary>
    public class MiniO : MinioClient, IFileStorage
    {
        private readonly ILogger<MiniO> _logger;
        private const char PathDelimeter = '/';
        private const int PresignedLifeTimeSeconds = 60 * 3;
        private string MainUrl { get; }
        private string SSLUrl { get; }

        public MiniO(IConfiguration config, ILogger<MiniO> logger)
        {
            _logger = logger;
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
        }

        /// <summary>
        /// Разделяет строку с fileName на кортеж строк
        /// (имя бакета, имя файла без префикса бакета)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns> Two strings tuple (bucketName, fileNameWithoutBucketPrefix) </returns>
        /// <exception cref="ArgumentException"></exception>
        private (string bucketName, string destinationPathWithoutBucketPrefix) ParseFileName(string fileName)
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
            var destinationPathWithoutBucketPrefix = fileName.Replace(bucketName + PathDelimeter, "");

            return (bucketName, destinationPathWithoutBucketPrefix);
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
            catch (MinioException ex)
            {
                _logger.LogError(ex, DateTime.UtcNow.ToLongTimeString());
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

        /// <inheritdoc/>
        public async Task<MemoryStream> GetFileStreamAsync(string destinationFileFullName)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(destinationFileFullName);

            var isBucketExist = await BucketExistAsync(bucketName);
            if (!isBucketExist)
            {
                throw new ArgumentException($"Нет бакета с таким именем: {bucketName}");
            }

            var isFileExists = await ObjectExistAsync(bucketName, fileNameWithoutBucketPrefix);
            if (!isFileExists)
            {
                throw new ArgumentException($"Не найден файл: {fileNameWithoutBucketPrefix}");
            }

            var ms = new MemoryStream();
            try
            {
                await GetObjectAsync(new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileNameWithoutBucketPrefix)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(ms);
                    }));
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, DateTime.UtcNow.ToLongTimeString());
            }

            return ms;
        }

        /// <inheritdoc/>
            public async Task<byte[]> GetFileBytesAsync(string destinationFileFullName)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(destinationFileFullName);

            var isBucketExist = await BucketExistAsync(bucketName);
            if (!isBucketExist)
            {
                throw new ArgumentException($"Нет бакета с таким именем: {bucketName}");
            }

            var isFileExists = await ObjectExistAsync(bucketName, fileNameWithoutBucketPrefix);
            if (!isFileExists)
            {
                throw new ArgumentException($"Не найден файл: {fileNameWithoutBucketPrefix}");
            }

            using (var ms = new MemoryStream())
            {
                try
                {
                    await GetObjectAsync(new GetObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(fileNameWithoutBucketPrefix)
                        .WithCallbackStream(stream =>
                        {
                            stream.CopyToAsync(ms).Wait();
                        }));
                }
                catch (MinioException ex)
                {
                    _logger.LogError(ex, DateTime.UtcNow.ToLongTimeString());
                }

                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }

        /// <inheritdoc/>
        public string GetFileLink(string destinationFileFullName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<string> GetFileLinkAsync(string destinationFileFullName)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(destinationFileFullName);

            var isBucketExist = await BucketExistAsync(bucketName);
            if (!isBucketExist)
            {
                throw new ArgumentException($"Нет бакета с таким именем: {bucketName}");
            }

            var isFileExists = await ObjectExistAsync(bucketName, fileNameWithoutBucketPrefix);
            if (!isFileExists)
            {
                throw new ArgumentException($"Не найден файл: {fileNameWithoutBucketPrefix}");
            }

            try
            {
                String url = await PresignedGetObjectAsync(new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileNameWithoutBucketPrefix)
                    .WithExpiry(PresignedLifeTimeSeconds));
                return url;
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, DateTime.UtcNow.ToLongTimeString());
                throw;
            }
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
            else
            {
                // Удаляем предыдущую версию файла
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

        /// <inheritdoc/>
        public async Task DeleteFileAsync(string filename)
        {
            var (bucketName, fileNameWithoutBucketPrefix) = ParseFileName(filename);

            await DeleteFile(bucketName, fileNameWithoutBucketPrefix);
        }

        /// <inheritdoc/>
        public async Task<List<string>> ListFilesAsync(string destinationFolderFullName)
        {
            var tokens = destinationFolderFullName.Split(PathDelimeter);
            var bucketName = tokens[0];
            var prefix = "";
            if (tokens.Length > 1)
            {
                prefix = destinationFolderFullName.Replace(bucketName + PathDelimeter, "");
            }

            var filePaths = new List<string>();
            try
            {
                var listArgs = new ListObjectsArgs()
                    .WithBucket(destinationFolderFullName)
                    .WithPrefix(prefix)
                    .WithRecursive(true);

                // Слушаем поток объектов и добавляем их в список
                await foreach (var item in ListObjectsEnumAsync(listArgs))
                {
                    if (!item.IsDir)
                    {
                        filePaths.Add($"{destinationFolderFullName}/{item.Key}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, DateTime.UtcNow.ToLongTimeString());
            }

            return filePaths;
        }
    }
}