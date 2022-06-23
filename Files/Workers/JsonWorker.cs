using System;
using System.Collections.Generic;
using System.Linq;
using JobMarket.Files.Interfaces;
using JobMarket.Files.Settings;
using JobMarket.Models;
using Microsoft.Extensions.Options;

namespace JobMarket.Files.Workers
{
    internal class GenericJsonWorker<T> : IGenericStorageWorker<T>
        where T : BaseModel
    {
        private readonly IReaderWriter _readerWriter;
        private readonly string _storagePath;

        public GenericJsonWorker(IReaderWriter readerWriter, IOptions<JsonDBSettings> settings)
        {
            _readerWriter = readerWriter;
            _storagePath = settings.Value is not null ? GetFilePath(settings.Value)
                                                          : throw new ArgumentNullException(nameof(settings));
        }

        public BaseModel GetById(Guid id)
        {
            return (this.GetByCondition(u => u.Id == id)).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return this._readerWriter.Read<IEnumerable<T>>(this._storagePath);
        }

        public IEnumerable<T> GetByCondition(Func<T, bool> condition)
        {
            return this.GetAll().Where(condition);
        }
        
        public void Create(T entity)
        {
            var data = this.GetAll().ToList();
            data.Add(entity);
            this._readerWriter.Write(this._storagePath, data);
        }

        public void Delete(T entity)
        {
            var data = this.GetAll().ToList();
            var item = data.FirstOrDefault(x => x.Id == entity.Id);
            data.Remove(item);
            this._readerWriter.Write(this._storagePath, data);
        }

        public void Update(T entity)
        {
            var data = this.GetAll().ToList();
            var index = data.IndexOf(data.FirstOrDefault(x => x.Id == entity.Id));
            data[index] = entity;
            this._readerWriter.Write(this._storagePath, data);
        }

        public string GetFilePath(JsonDBSettings settings)
        {
            string removePostfix = "Model", addPostfix = "Directory";
            var propertyName = typeof(T).Name.Replace(removePostfix, addPostfix);
            return settings.GetType()
                           .GetProperty(propertyName)
                           ?.GetValue(settings)
                           ?.ToString()
                           ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}

