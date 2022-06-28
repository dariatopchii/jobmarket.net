using System;
using System.Collections.Generic;
using JobMarket.Models;

namespace JobMarket.Files.Workers
{
    public interface IGenericCollection<T>
        where T : BaseModel
    {
        string StoragePath { get; set; }
        
        List<T> Collection { get; set; }
        
        List<T> GetByCondition(Func<T, bool> condition);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void ReadFromFile();

        void SaveToFile();
    }
}