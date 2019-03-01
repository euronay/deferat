using Deferat.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Deferat.Services
{

    public class AuthorService : IAuthorService
    {
        public IEnumerable<Author> Authors { get; set; }

        private ILogger _logger;
        private IFileReader<Author> _fileReader;

        public AuthorService(ILogger<AuthorService> logger, IFileReader<Author> fileReader)
        {
            _logger = logger;
            _fileReader = fileReader;
        }

        public Author GetAuthor(string id)
        {
            return Authors.FirstOrDefault(a => a.Id == id);
        }

        public void LoadAuthors(string path)
        {
            _logger.LogInformation($"Loading authors from {path}...");

            var authorFiles = Directory.GetFiles(path, "*.md");

            var authors = new List<Author>();
            foreach(var authorFile in authorFiles)
            {                   
                authors.Add(ReadAuthorFromFile(authorFile));
            }
             
            Authors = authors.OrderBy(a => a.DisplayName);
        }

        private Author ReadAuthorFromFile(string path)
        {
            _logger.LogInformation($"Loading {path}");

            var author = _fileReader.ReadFile(path);

            _logger.LogInformation($"Loaded {author.Id}");

            return author;
        }

        
    }
    
}