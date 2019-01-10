
using Deferat.Services;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index()
        {
            return View(_postService.Posts);
        }

    }
}