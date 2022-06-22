using System.IO;
using JobMarket.Files.Interfaces;
using Newtonsoft.Json;

namespace JobMarket.Files.ReaderWriters
{
    internal class JsonReaderWriter : IReaderWriter
    {
        public T Read<T>(string source)
        {
            var data = File.ReadAllText(source);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public void Write<T>(string source, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            File.WriteAllText(source, json);
        }
    }
}

