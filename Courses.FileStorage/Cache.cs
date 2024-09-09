namespace Courses.FileStorage
{
    public sealed class Cache<T>(int lifeTimeSecond = 600, int itemLimit = 100)
    {
        private readonly int? _lifeTimeSeconds = lifeTimeSecond;

        private readonly int _itemCountLimit = itemLimit * 1000 * 1000;
        public int Size => Data.Count;
        private Dictionary<string, (T data, DateTime date)> Data { get; set; } = new();

        public T? this[string i]
        {
            get => Get(i);
            set => Add(i, value);
        }

        public void Add(string key, T? data)
        {
            var now = DateTime.Now;
            if (Data.ContainsKey(key)) return;
            Data.TryAdd(key, (data, now));

            if (Size < _itemCountLimit) return;
            Data = new();
        }

        public T? Get(string key)
        {
            if (!Data.ContainsKey(key)) return default;

            if ((DateTime.Now - Data[key].date).Seconds > _lifeTimeSeconds)
            {
                Data.Remove(key);
                return default;
            }

            return Data[key].data;
        }
    }
}