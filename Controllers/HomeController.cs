using Deferat.Models;
using Deferat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace Deferat.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private IPostService _postService;
        private IAuthorService _authorService;
        public HomeController(ILogger<HomeController> logger, IPostService postService, IAuthorService authorService)
        {
            _logger = logger;
            _postService = postService;
            _authorService = authorService;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Hello, World");

            ViewData["Posts"] = _postService.Posts.Take(4);

            return View();
        }

        public IActionResult About()
        {
            var authors = _authorService.Authors;

            return View(authors);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
