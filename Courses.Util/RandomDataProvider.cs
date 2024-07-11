namespace Courses.Util
{
    public static class RandomDataProvider
    {
        /// <summary>
        /// Возвращает массив случайных байт
        /// </summary>
        /// <param name="sizeInKb"></param>
        /// <returns></returns>
        public static byte[] GetByteArray(int sizeInKb)
        {
            Random rnd = new Random();
            byte[] b = new byte[sizeInKb * 1024]; // convert kb to byte
            rnd.NextBytes(b);
            return b;
        }
    }
}
