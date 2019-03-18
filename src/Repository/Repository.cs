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
        private string _path;
        private Func<T,T> _processor;

        private IQueryable<T> Dataset
        {
            get
            {
                if (_dataSet == null)
                {
                    _dataSet = LoadData();
                }
                return _dataSet;
            }
            set
            {
                _dataSet = value;
            }
        }

        public Repository(string path, Func<T, T> postProcessor, ILogger<Repository<T>> logger, IFileReader<T> fileReader)
        {
            _logger = logger;
            _fileReader = fileReader;
            _path = path;
            _processor = postProcessor;
            _logger.LogInformation($"Loading data files from {path}...");
        }

        public T Get(string id)
        {
            // throw notinitializedexception?
            return Dataset.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            // throw notinitializedexception?
            var query = Dataset;

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

        private IQueryable<T> LoadData()
        {
            var directories = Directory.GetDirectories(_path);

            var dataList = new List<T>();
            foreach(var directory in directories)
            {
                var file = Directory.GetFiles(directory, "*.md").FirstOrDefault();
                if(file == null)
                    continue;
                    
                var dataFile = _fileReader.ReadFile(file);
                // TODO - this is a bit messy - set the ID of the article to be the directory name
                dataFile.Id = new FileInfo(directory).Name.ToLower();
                if(_processor != null)
                    dataFile = _processor(dataFile);
                
                dataList.Add(dataFile);                
            }

            return dataList.AsQueryable();
        }
    }
}