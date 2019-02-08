
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
        public ActionResult Index(int pageNumber = 1)
        {
            var viewModel = new PostsViewModel()
            {
                Posts = _postService.GetPosts(pageNumber),
                PageCount = _postService.PageCount,
                CurrentPage = pageNumber
            };

            return View(viewModel);
        }

        public ActionResult ViewPost(string id)
        {
            var post = _postService.Posts.FirstOrDefault(p => p.Locator.ToLower() == id.ToLower());
            if(post != null)
                return View(post);

            throw new System.Exception("?");
        }

    }
}