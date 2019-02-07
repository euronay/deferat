using Deferat.Models;
using System.Collections.Generic;

namespace Deferat.Services
{
    public interface IPostService
    {
        IEnumerable<PostModel> Posts { get; set; }
        void LoadPosts(string path);

        int PageCount { get; }
        int PostCount { get; }

        IEnumerable<PostModel> GetPosts(int pageNo);
    }
}