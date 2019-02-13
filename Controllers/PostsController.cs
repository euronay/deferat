
using Deferat.Services;
using Deferat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Deferat.Controllers
{
    public class PostsController : Controller
    {
        private IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: Posts
        public ActionResult Index(string tag, int pageNumber = 1)
        {
            var pageCount = 0;
            var posts = _postService.GetPosts(pageNumber, out pageCount, tag);

            var viewModel = new PostsViewModel()
            {
                Posts = posts,
                PageCount = pageCount,
                CurrentPage = pageNumber
            };

            return View(viewModel);
        }

        public ActionResult Read(string id)
        {
            var post = _postService.Posts.FirstOrDefault(p => p.Locator.ToLower() == id.ToLower());
            if(post != null)
                return View(post);

            throw new System.Exception("?");
        }

    }
}