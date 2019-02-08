using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.Services
{
    public interface IPostService
    {
        IEnumerable<Post> Posts { get; set; }
        void LoadPosts(string path);

        int PageCount { get; }
        int PostCount { get; }

        IEnumerable<Post> GetPosts(int pageNo);
    }
}