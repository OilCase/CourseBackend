namespace Courses.FileStorage
{
    /// <summary>
    /// Представляет интерфейс над объектным/файловым хранилищем
    /// </summary>
    public interface IFileStorage
    {
        ///// <summary>
        ///// Возвращает сгенерированную ссылку на скачивание файла по имени файла
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns> Строка со ссылкой</returns>
        //public string GetFileLink(string filename);

        ///// <summary>
        ///// Возвращает кортеж (массив байт, строка), представляющий файл и content-type файла
        ///// по имени файла
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns>Кортеж (массив байт, строка)</returns>
        //public (byte[] Bytes, string ContentType) GetFile(string filename);

        ///// <summary> проверяет существование файла </summary>
        ///// <param name="filename"></param>
        ///// <returns>bool (существует / не существует)</returns>
        //public bool FileExist(string filename);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        public void UploadFile(byte[] file, string path, string fileName);

        //public void DeleteFile(string filename);

        ///// <summary>
        ///// Возвращает список строк с именами файлов,
        ///// расположенных по определенному пути в хранилище
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns>Массив строк</returns>
        //public List<string> ListFiles(string path);
    }
}