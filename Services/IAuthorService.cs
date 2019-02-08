using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.Services
{
    public interface IAuthorService
    {
        IEnumerable<Author> Authors { get; set; }
        void LoadAuthors(string path);
        Author GetAuthor(string id);
    }
}