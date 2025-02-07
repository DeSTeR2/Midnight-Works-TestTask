namespace Utils
{
    public interface ILoadable
    {
        public string FileName { get; }

        public bool Load();
        public void Assign<T>(T data) where T : IData;
    }
}