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

        public void SaveData(string value)
        {
            string path = "./newfile.txt";
            
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                }	
            }
            // var path = "./newfile.txt";
            // File.Create(path);
            // var sw = new StreamWriter(path);
            // sw.Write(path);
            // File.WriteAllText(path,value);
        }
    }
}

