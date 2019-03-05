using Deferat.Models;
using Deferat.Repository;
using Deferat.Services;
using Deferat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace Deferat.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private IRepositoryContainer _repositories;

        public HomeController(ILogger<HomeController> logger, IRepositoryContainer repositories)
        {
            _logger = logger;
            _repositories = repositories;
        }
        public IActionResult Index()
        {
            ViewData["Posts"] = _repositories.Posts.Get(orderBy: list => list.OrderByDescending(post => post.Date)).Take(4).Select(p => new PostViewModel(){Post = p});

            return View();
        }

        public IActionResult About()
        {
            var authors = _repositories.Authors.Get();

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
