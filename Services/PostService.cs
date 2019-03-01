using Deferat.Models;
using Markdig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Deferat.Services
{

    public class PostService : IPostService
    {
        private const int PostsPerPage = 3;

        public IEnumerable<Post> Posts { get; set; }
        private ILogger _logger;
        private IFormatterService _formatter;
        private IFileReader<Post> _fileReader;

        public IEnumerable<Post> GetPosts(int pageNo, out int pageCount, string tag = null)
        {
            var posts = Posts;
            if(!String.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.Categories.Contains(tag));
            }
            
            pageCount = (posts.Count() - 1) / PostsPerPage + 1;

            if (pageNo > pageCount)
                throw new ArgumentException($"Requested page {pageNo} but there are only {pageCount} pages");

            return posts.Skip((pageNo - 1) * PostsPerPage).Take(PostsPerPage);
        }

        public PostService(ILogger<PostService> logger, IFormatterService formatter, IFileReader<Post> fileReader)
        {
            _logger = logger;
            _formatter = formatter;
            _fileReader = fileReader;
        }

        public void LoadPosts(string path)
        {
            _logger.LogInformation($"Loading posts from {path}...");

            var postDirectories = Directory.GetDirectories(path);

            var posts = new List<Post>();
            foreach(var directory in postDirectories)
            {
                var postFile = Directory.GetFiles(directory, "*.md").FirstOrDefault();
                if(postFile == null)
                    continue;
                    
                posts.Add(ReadPostFromFile(postFile));
            }
             
            Posts = posts.OrderByDescending(p => p.Date);
        }

        private Post ReadPostFromFile(string path)
        {
            _logger.LogInformation($"Loading {path}");

            var post = _fileReader.ReadFile(path);

            post.Id = Path.GetFileName(Path.GetDirectoryName(path));

            post.Html = _formatter.FixImages(post.Html, post.Id);;
            post.Image = $"/posts/{post.Id}/{post.Image}";
            post.ShortContent = _formatter.CreateTruncatedContent(post.Html, 200);

            _logger.LogInformation($"Loaded {post.Title}");

            return post;
        }

        
    }
    
}