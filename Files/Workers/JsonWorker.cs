using System;
using System.Collections.Generic;
using System.Linq;
using JobMarket.Files.Interfaces;
using JobMarket.Files.Settings;
using JobMarket.Models;
using Microsoft.Extensions.Options;

namespace JobMarket.Files.Workers
{
    internal class JsonWorker<T> : IStorageWorker<T>
        where T : BaseModel
    {
        private readonly IReaderWriter _readerWriter;
        private readonly string _storagePath;

        public JsonWorker(IReaderWriter readerWriter, IOptions<JsonDbSettings> settings)
        {
            _readerWriter = readerWriter;
            _storagePath = settings.Value is not null ? GetFilePath(settings.Value)
                                                          : throw new ArgumentNullException(nameof(settings));
        }
        
        public IEnumerable<T> GetAll()
        {
            return _readerWriter.Read<IEnumerable<T>>(_storagePath);
        }

        public IEnumerable<T> GetByCondition(Func<T, bool> condition)
        {
            return GetAll().Where(condition);
        }
        
        public void Create(T entity)
        {
            var data = GetAll().ToList();
            data.Add(entity);
            this._readerWriter.Write(_storagePath, data);
        }

        public void Delete(T entity)
        {
            var data = GetAll().ToList();
            var item = data.FirstOrDefault(x => x.Id == entity.Id);
            data.Remove(item);
            this._readerWriter.Write(_storagePath, data);
        }

        public void Update(T entity)
        {
            var data = GetAll().ToList();
            var index = data.IndexOf(data.FirstOrDefault(x => x.Id == entity.Id));
            data[index] = entity;
            _readerWriter.Write(_storagePath, data);
        }

        public string GetFilePath(JsonDbSettings settings)
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

