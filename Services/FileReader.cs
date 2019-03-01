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
    public class FileReader<T> : IFileReader<T> where T : class, IMetadata
    {
        public T ReadFile(string path)
        {
            (string rawMetadata, string rawMarkDown) = ReadFileToText(path);

            T metadata = DeserializeMetadata(rawMetadata);

            metadata.Html = ParseMarkdown(rawMarkDown);

            return metadata;
        }

        private (string metadata, string markdown) ReadFileToText(string path)
        {
            string metadata = string.Empty;
            string html = string.Empty;

            using(var reader = File.OpenText(path))
            {
                if(reader.ReadLine() != "---")
                    throw new FormatException("File was not in expected format");
                
                string line;

                while((line = reader.ReadLine()) != "---")
                {
                    metadata += line + "\r\n";
                }

                html = reader.ReadToEnd();
            }

            return (metadata, html);
        }

        private T DeserializeMetadata(string metaData)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<T>(new StringReader(metaData));
        }

        private string ParseMarkdown(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoIdentifiers()
                .Build();
            string html = Markdown.ToHtml(markdown, pipeline);
            return html;
        }
    } 
}