using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Deferat.Services
{
    public class FormatterService : IFormatterService
    {
        private ILogger _logger;
        public FormatterService(ILogger<FormatterService> logger)
        {
            _logger = logger;
        }

        public string CreateTruncatedContent(string html, int maxLength)
        {
            // TODO make this better!
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var shortDoc = new HtmlDocument();
            // extract text
            string text = string.Empty;
            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
            {
                
                text += node.InnerText;
                shortDoc.DocumentNode.AppendChild(node);

                _logger.LogInformation($"Text: {text}");
                _logger.LogInformation($"Html {shortDoc.DocumentNode.OuterHtml}");

                if(text.Length > maxLength || node.FirstChild?.Name == "img")
                    return shortDoc.DocumentNode.OuterHtml;
            }

            // text is shorted than max
            return htmlDoc.DocumentNode.OuterHtml;

        }

        public string FixImages(string html, string folder)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img");

            if(imageNodes != null)
            {
                foreach (var imageNode in imageNodes)
                {
                    var src = imageNode.Attributes["src"].Value;
                    src = $"/posts/{folder}/{src}";
                    imageNode.SetAttributeValue("src", src);

                    imageNode.Attributes.Add("class", "img-fluid d-block mx-auto shadow-sm rounded");
                }
            }

            var tableNodes = htmlDoc.DocumentNode.SelectNodes("//table");
            if(tableNodes != null)
            {
                foreach (var tableNode in tableNodes)
                {
                    tableNode.Attributes.Add("class", "table");
                }
            }

            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}
