using System.Collections.Generic;

namespace Deferat.ViewModels
{
    public class PostListViewModel : ViewModelBase
    {
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string Tag { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
