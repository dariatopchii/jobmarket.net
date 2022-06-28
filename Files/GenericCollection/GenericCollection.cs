using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JobMarket.Files.Workers;
using JobMarket.Models;
using Newtonsoft.Json;

namespace JobMarket.Files.GenericCollection
{
    internal class GenericCollection<T>: IGenericCollection<T>
        where T : BaseModel
    {
        public string StoragePath { get; set; }

        public List<T> Collection { get; set; }
        
        public void ReadFromFile()
        {
            var data = File.ReadAllText(StoragePath);
            Collection =  JsonConvert.DeserializeObject<List<T>>(data);
        }
        

        public List<T> GetByCondition(Func<T, bool> condition)
        {
            return Collection.Where(condition).ToList();
        }
        
        public void Create(T entity)
        {
            Collection.Add(entity);
        }

        public void Delete(T entity)
        {
            var item = Collection.FirstOrDefault(x => x.Id == entity.Id);
            Collection.Remove(item);
        }

        public void Update(T entity)
        {
            var index = Collection.IndexOf(Collection.FirstOrDefault(x => x.Id == entity.Id));
            Collection[index] = entity;
        }

        public void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(Collection);
            File.WriteAllText(StoragePath, json);
        }
    }
}

