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
        public HomeController(ILogger<HomeController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Hello, World");

            ViewData["Posts"] = _postService.Posts.Take(3);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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
