using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.ViewModels
{
    public class PostsViewModel
    {
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}
