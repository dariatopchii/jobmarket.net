using System;
using System.Collections.Generic;
using JobMarket.Models;

namespace JobMarket.Files.Interfaces
{
    public interface IGenericStorageWorker<T>
     where T : BaseModel
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetByCondition(Func<T, bool> condition);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}

