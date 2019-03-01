using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Deferat.Models;
using Deferat.Services;
using Microsoft.Extensions.Logging;

namespace Deferat.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IMetadata
    {
        private ILogger _logger;
        private IFileReader<T> _fileReader;
        private IQueryable<T> _dataSet;

        public Repository(ILogger<Repository<T>> logger, IFileReader<T> fileReader)
        {
            _logger = logger;
            _fileReader = fileReader;
        }

        public void Initialize(string path, Func<T, T> postProcessor = null)
        {
            _logger.LogInformation($"Loading data files from {path}...");

            var directories = Directory.GetDirectories(path);

            var dataList = new List<T>();
            foreach(var directory in directories)
            {
                var file = Directory.GetFiles(directory, "*.md").FirstOrDefault();
                if(file == null)
                    continue;
                    
                var dataFile = _fileReader.ReadFile(file);
                if(postProcessor != null)
                    dataFile = postProcessor(dataFile);
                
                dataList.Add(dataFile);                
            }

            _dataSet = dataList.AsQueryable();
        }

        public T Get(string id)
        {
            // throw notinitializedexception?
            return _dataSet.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            // throw notinitializedexception?
            var query = _dataSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsEnumerable();
            }

            return query.AsEnumerable();
        }
    }
}