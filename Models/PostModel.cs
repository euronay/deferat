using System.Collections.Generic;

namespace Deferat.Models
{
    public class PostModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string Content { get; set; }
    }

}