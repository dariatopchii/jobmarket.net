namespace JobMarket.Files.Interfaces
{
    public interface IReaderWriter
    {
        T Read<T>(string source);
        void Write<T>(string source, T value);
    }
}

