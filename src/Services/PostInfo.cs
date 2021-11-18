using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace Deferat.Services
{
    public class PostInfo : IPostInfo
    {
        public int GetTimeToRead(string postHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(postHtml);
            var documentNodeInnerText = htmlDoc.DocumentNode.InnerText;
            MatchCollection collection = Regex.Matches(documentNodeInnerText, @"[\S]+");
            var wordCount = collection.Count;
            var readTime = Math.Max((wordCount / 200), 1);
            return readTime;
        }
    }
}