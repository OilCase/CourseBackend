using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Courses.FileStorage
{
    /// <summary>
    /// Класс <c>MiniO</c> реализует интерфейс IFileStorage для файлового хранилища МиниО.
    /// </summary>
    public class MiniO : MinioClient, IFileStorage
    {
        /// <param name="config"> </param>
        ///
        private Cache<(byte[] data, string contentType)> _cache { get; init; }

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

            _cache = new(100);
        }


        /// <summary>
        /// Этот метод возвращает сгенерированную ссылку на скачивание файла из MiniO.
        /// Если файл не найден - вернёт пустую строку
        /// </summary>
        /// <param name="filename">Имя файла в формате "имя бакета/имя файла (возможно, с префиксами)"</param>
        /// <returns>Строка, содержащую ссылку на скачивание файла</returns>
        public string GetFileLink(string filename)
        {
            var splittedFileName = filename.Split(['/'], 2);
            var bucket = splittedFileName[0];
            var fileName = splittedFileName[1];

            PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithExpiry(60 * 60 * 24);

            string url = FileExist(filename)
                ? PresignedGetObjectAsync(args).Result
                : "";

            return url;
        }

        /// <summary>
        /// Этот метод возвращает файл и content-type файла из MiniO
        /// </summary>
        /// <param name="filename">Имя файла в формате "имя бакета/имя файла(возможно, с префиксами)"</param>
        /// <returns>Кортеж, содержащий сам файл, представленный массивом байт, и строку, содержащую content-type</returns>
        public (byte[], string) GetFile(string filename)
        {
            var cacheObject = _cache[filename];
            if (cacheObject != default)
                return cacheObject;

            var splittedFileName = filename.Split(['/'], 2);
            var bucket = splittedFileName[0].ToLower();
            var fileName = splittedFileName[1];

            // Параметры для получения информации по файлу
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName);

            var statResult = StatObjectAsync(statObjectArgs).Result;
            var contentType = statResult.ContentType;

            MemoryStream destination = new();

            // Параметры для получения файла (копируем файл в память)
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithCallbackStream((stream) => { stream.CopyTo(destination); });
            GetObjectAsync(getObjectArgs).Wait();

            var data = destination.ToArray();

            _cache[filename] = (data, contentType);
            return (data, contentType);
        }

        public bool FileExist(string filename) => IsObjectExist(filename.Split('/', 2)[0], filename.Split('/', 2)[1]);


        /// <summary>
        /// Этот метод возвращает имена файлов, которые содержатся в определённом бакете.
        /// Имя бакета не добавляется к возвращаемым именам файлов.
        /// </summary>
        /// <param name="path">Строка, содержащая путь до интересующей нас директории в МиниО. 
        ///                    Может содержать префиксы("bucketName/prefix1/prefix2"), 
        ///                    тогда имена файлов на выходе будут отфильтрованы по префиксам</param>
        /// <returns></returns>
        public List<string> ListFiles(string path)
        {
            string bucket = path;
            string prefix = "";
            if (path.Contains("/"))
            {
                var splittedPath = path.Split(new[] { '/' }, 2);
                bucket = splittedPath[0];
                prefix = splittedPath[1];
            }

            ListObjectsArgs args = new ListObjectsArgs()
                .WithBucket(bucket)
                .WithRecursive(true);

            var fileNames = ListObjectsEnumAsync(args)
                .Select(fn => fn.Key)
                .Where(fn => fn.StartsWith(prefix))
                .ToListAsync()
                .Result;

            return fileNames;
        }

        /// <summary>
        /// Загружает файл в объектное хранилище МиниО.
        /// В случае, если бакет, определенный в fileName не существует, 
        /// или файл с таким именем уже есть в бакете - выбрасывает исключение
        /// </summary>
        /// <param name="fileBytes">Байтовое представление файла</param>
        /// <param name="path">Имя файла в формате "имя бакета//имя файла (можно с префиксами)"</param>
        /// <param name="contentType">Строка с MIME-типом файла"</param>
        public void UploadFile(byte[] fileBytes, string path, string contentType)
        {
            if (!path.Contains('/'))
                throw new ArgumentException("Имя (path) должно содержать имя бакета до '/'");
            var splittedFileName = path.Split(['/'], 2);


            var bucket = splittedFileName[0];
            var fileName = splittedFileName[1];


            if (!IsBucketExist(bucket)) MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));

            if (IsObjectExist(bucket, fileName)) DeleteFile(path);

            var stream = new MemoryStream(fileBytes);

            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType)
                .WithStreamData(stream);

            var result = PutObjectAsync(putObjectArgs).Result;
        }

        /// <summary> Загружает файл в объектное хранилище МиниО.
        /// В случае, если бакет, определенный в fileName не существует, 
        /// или файл с таким именем уже есть в бакете - выбрасывает исключение </summary>
        /// <param name="fileBytes">Байтовое представление файла</param>
        /// <param name="path">Имя файла в формате "имя бакета//имя файла (можно с префиксами)"</param>
        /// <param name="contentType">Строка с MIME-типом файла"</param>
        public void UploadFile(Stream stream, string path, string contentType)
        {
            if (!path.Contains('/'))
                throw new ArgumentException("Имя (path) должно содержать имя бакета до '/'");

            var splittedFileName = path.Split(['/'], 2);

            var bucket = splittedFileName[0];
            var fileName = splittedFileName[1];


            if (!IsBucketExist(bucket)) MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));

            if (IsObjectExist(bucket, fileName)) DeleteFile(path);

            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType)
                .WithStreamData(stream);

            var result = PutObjectAsync(putObjectArgs).Result;
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