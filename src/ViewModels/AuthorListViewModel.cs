using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.ViewModels
{
    public class AuthorListViewModel : ViewModelBase
    {
        public IEnumerable<Author> Authors { get; set; }
    }
}
