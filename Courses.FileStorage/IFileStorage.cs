namespace Courses.FileStorage
{
    /// <summary>
    /// Представляет интерфейс над объектным/файловым хранилищем
    /// </summary>
    public interface IFileStorage
    {
        /// <summary> Возвращает ссылку на скачивание файла </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public string GetFileLink(string destinationFileFullName);

        /// <summary>
        /// Возвращает true если файл с destinationFileFullName
        /// существует в хранилище
        /// </summary>
        /// <param name="destinationFileFullName"> Имя файла со всеми префиксами и расширением </param>
        public bool FileExist(string destinationFileFullName);

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
        public void DeleteFile(string destinationFileFullName);

        /// <summary>
        /// Возвращает список полных имён файлов,
        /// содержащихся в директории destinationFolderFullName
        /// </summary>
        /// <param name="destinationFolderFullName"> Имя директории в хранилище со всеми префиксами </param>
        public List<string> ListFiles(string destinationFolderFullName);
    }
}