using System;
using System.Collections.Generic;

namespace Deferat.Models
{
    public class Post : IMetadata
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string Html { get; set; }
        public string Image {get; set;}
        public bool Featured { get; set; }
        public string ShortContent { get; set; }
    }

}