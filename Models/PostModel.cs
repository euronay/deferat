using System;
using System.Collections.Generic;

namespace Deferat.Models
{
    public class PostModel
    {
        public string Locator { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string Content { get; set; }
        public string Image {get; set;}
    }

}