using Microsoft.Extensions.Configuration;

namespace Courses.FileStorage
{
    public class LocalStorage: IFileStorage
    {
        protected readonly string _contentPath;
        //protected readonly IConfiguration configuration;

        public LocalStorage(IConfiguration configuration)
        {
            _contentPath = $"{configuration.GetValue<string>("CourseContentDirectoryPath")}";
        }

        /// <summary>
        /// Загружает файл в указанную директорию на сервере.
        /// Если директории нет, создаёт путь до неё целиком.
        /// Возвращает путь к файлу относительно папки содержимого 
        /// сайта на сервере.
        /// </summary>
        public async void UploadFile(byte[] fileBytes, string path, string fileName)
        {
            var fullPath = Path.Combine(_contentPath, path);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string filePath = Path.Combine(_contentPath, path, fileName);
            if (System.IO.File.Exists(filePath))
            {
                throw new IOException($"Файл уже существует: {fileName}");
            }

            await using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(fileBytes);
                //await file.CopyToAsync(fileStream);
            }
        }

        public List<string> ListFiles(string path)
        {
            throw new NotImplementedException();
        }

        //public async Task<string> UploadFile(string path, IFormFile file)
        //{
        //    var fullPath = Path.Combine(_contentPath, path);

        //    if (!Directory.Exists(fullPath))
        //        Directory.CreateDirectory(fullPath);

        //    string filePath = Path.Combine(_contentPath, path, file.FileName);
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        throw new IOException($"Файл уже существует: {file.FileName}");
        //    }

        //    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }

        //    return filePath;
        //}
    }
}
