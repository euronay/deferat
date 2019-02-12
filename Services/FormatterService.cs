using HtmlAgilityPack;

namespace Deferat.Services
{
    public class FormatterService : IFormatterService
    {

        public string CreateTruncatedContent(string html, int maxLength)
        {
            // TODO make this better!
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // extract text
            string text = string.Empty;

            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//text()"))
            {
                text += node.InnerText;
            }


            if (text.Length <= maxLength)
                return text;

            var trimmedText = text.Substring(0, maxLength);
            trimmedText = trimmedText.Substring(0, trimmedText.LastIndexOf(" "));

            return $"{trimmedText}...";

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

                    imageNode.Attributes.Add("class", "img-fluid");
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
