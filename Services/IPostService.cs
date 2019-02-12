using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.Services
{
    public interface IPostService
    {
        IEnumerable<Post> Posts { get; set; }
        void LoadPosts(string path);

        IEnumerable<Post> GetPosts(int pageNo, out int pageCount, string tag = null);
    }
}