using System.Collections.Generic;
using Deferat.Models;

namespace Deferat.Services
{
    public interface IPostService
    {
        IEnumerable<PostModel> Posts { get; set; }
        void LoadPosts(string path);
    }
}