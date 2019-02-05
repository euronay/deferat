using Deferat.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
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

            content = FixImages(content, post.Locator);
            
            post.Content = content;

            _logger.LogInformation($"Loaded {post.Title}");

            return post;
        }

        private string FixImages(string html, string folder)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img");
            if (imageNodes == null)
                return html;

            foreach(var imageNode in imageNodes)
            {
                var src = imageNode.Attributes["src"].Value;
                src = $"/posts/{folder}/{src}";
                imageNode.SetAttributeValue("src", src);

                imageNode.Attributes.Add("class", "img-fluid");
            }

            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
    
}