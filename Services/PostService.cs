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

    public class PostService : IPostService
    {
        private const int PostsPerPage = 3;

        public IEnumerable<PostModel> Posts { get; set; }

        private ILogger _logger;
        private IFormatterService _formatter;

        public int PostCount
        {
            get
            {
                return Posts.Count();
            }
        }

        public int PageCount
        {
            get
            {
                return (PostCount - 1) / PostsPerPage + 1;
            }
        }

        public IEnumerable<PostModel> GetPosts(int pageNo)
        {
            if (pageNo > PageCount)
                throw new ArgumentException($"Requested page {pageNo} but there are only {PageCount} pages");

            return Posts.Skip((pageNo - 1) * PostsPerPage).Take(PostsPerPage);
        }

        public PostService(ILogger<PostService> logger, IFormatterService formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        public void LoadPosts(string path)
        {
            _logger.LogInformation($"Loading posts from {path}...");

            var postDirectories = Directory.GetDirectories(path);

            var posts = new List<PostModel>();
            foreach(var directory in postDirectories)
            {
                var postFile = Directory.GetFiles(directory, "*.md").FirstOrDefault();
                if(postFile == null)
                    continue;
                    
                posts.Add(ReadPostFromFile(postFile));
            }
             
            Posts = posts.OrderByDescending(p => p.Date);
        }

        private PostModel ReadPostFromFile(string path)
        {
            _logger.LogInformation($"Loading {path}");

            string rawMetadata = string.Empty;
            string rawPost = string.Empty;

            using(var reader = File.OpenText(path))
            {
                if(reader.ReadLine() != "---")
                    return null;
                
                string line;

                while((line = reader.ReadLine()) != "---")
                {
                    rawMetadata += line + "\r\n";
                }

                rawPost = reader.ReadToEnd();
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            var post = deserializer.Deserialize<PostModel>(new StringReader(rawMetadata));

            post.Locator = Path.GetFileName(Path.GetDirectoryName(path));

            string content = Markdig.Markdown.ToHtml(rawPost);

            content = _formatter.FixImages(content, post.Locator);

            post.Content = content;
            post.ShortContent = _formatter.CreateTruncatedContent(content, 200);

            _logger.LogInformation($"Loaded {post.Title}");

            return post;
        }

        
    }
    
}