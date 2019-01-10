using System.Collections.Generic;
using System.IO;
using System.Linq;
using Deferat.Models;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using Markdig;
using YamlDotNet.Serialization.NamingConventions;

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

            post.Content = Markdig.Markdown.ToHtml(rawPost);

            _logger.LogInformation($"Loaded {post.Title}");

            return post;
        }
    }
}