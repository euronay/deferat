using System.Collections.Generic;

using Deferat.Models;

namespace Deferat.ViewModels
{
    public class AuthorListViewModel : ViewModelBase
    {
        public IEnumerable<Author> Authors { get; set; }
    }
}
