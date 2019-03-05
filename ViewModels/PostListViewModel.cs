using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.ViewModels
{
    public class PostListViewModel
    {
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
