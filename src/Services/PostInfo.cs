﻿using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Deferat.Services
{
    public class PostInfo : IPostInfo
    {
        public double GetTimeToRead(string postHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(postHtml);
            var documentNodeInnerText = htmlDoc.DocumentNode.InnerText;
            MatchCollection collection = Regex.Matches(documentNodeInnerText, @"[\S]+");
            var wordCount = collection.Count;
            var readTime = Math.Round((double)wordCount / 200);
            return readTime;
        }
    }
}