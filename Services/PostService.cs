using System.Collections.Generic;
using System.IO;
using System.Linq;
using Deferat.Models;
using Microsoft.Extensions.Logging;
using Markdig;

namespace Deferat.Services
{

    public class PostService : IPostService
    {
        public IEnumerable<PostModel> Posts { get; set; }
        private ILogger _logger;
        public PostService(ILogger<PostService> logger)
        {
            _logger = logger;
        }

        public void LoadPosts(string path)
        {
            _logger.LogInformation($"Loading posts from {path}...");

            Posts = Directory.GetDirectories(path)
                    .Select(directory => Directory.GetFiles(directory, "*.md").First())?
                    .Select(post => ReadPostFromFile(post));
        }

        private PostModel ReadPostFromFile(string path)
        {
            _logger.LogInformation($"Loading {path}");

            return new PostModel()
            {
                Title = path,
                Content = Markdig.Markdown.ToHtml(path)
            };
        }
    }
}