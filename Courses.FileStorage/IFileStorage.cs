namespace Courses.FileStorage
{
    /// <summary>
    /// Представляет интерфейс над объектным/файловым хранилищем
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Возвращает содержимое файла с destinationFileFullName
        /// в виде массива байт
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task<byte[]> GetFileBytesAsync(string destinationFileFullName);

        /// <summary>
        /// Возвращает содержимое файла с destinationFileFullName
        /// в виде потока
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task<MemoryStream> GetFileStreamAsync(string destinationFileFullName);

        /// <summary> Возвращает ссылку на скачивание файла </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public string GetFileLink(string destinationFileFullName);

        /// <summary> Возвращает ссылку на скачивание файла </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task<string> GetFileLinkAsync(string destinationFileFullName);

        /// <summary>
        /// Возвращает ссылку на скачивание файла.
        /// Если файл не найден - создаёт пустой файл по указанному пути.
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task<string> GetFileLinkWithAutoCreateAsync(string destinationFileFullName);

        /// <summary>
        /// Возвращает true если файл с destinationFileFullName
        /// существует в хранилище
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task<bool> FileExistAsync(string destinationFileFullName);

        /// <summary>
        /// Загружает file в хранилище
        /// по маршруту, соответствующему destinationFileFullName
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task UploadFileAsync(byte[] file, string destinationFileFullName);

        /// <summary>
        /// Удаляет файл с destinationFileFullName из хранилища
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public Task DeleteFileAsync(string destinationFileFullName);

        /// <summary>
        /// Возвращает список полных имён файлов,
        /// содержащихся в директории destinationFolderFullName
        /// </summary>
        /// <param name="destinationFolderFullName"> Имя директории в хранилище со всеми префиксами </param>
        public Task<List<string>> ListFilesAsync(string destinationFolderFullName);
    }
}