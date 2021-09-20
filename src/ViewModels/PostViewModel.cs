using Deferat.Models;

namespace Deferat.ViewModels
{
    public class PostViewModel : ViewModelBase
    {
        public Post Post { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImage { get; set; }
        public string AuthorTwitter { get; set; }
    }
}