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
        private Settings _settings;

        public HomeController(ILogger<HomeController> logger, IRepositoryContainer repositories, ISiteInfo siteInfo)
        {
            _logger = logger;
            _repositories = repositories;
            _settings = siteInfo.Settings;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = _settings.Title;
            ViewData["Logo"] = $"/images/{_settings.Logo}";
            ViewData["Posts"] = _repositories.Posts.Get(orderBy: list => list.OrderByDescending(post => post.Date)).Take(4).Select(p => new PostViewModel(){Post = p});

            return View();
        }

        public IActionResult About()
        {
            var model = new AuthorListViewModel() { Settings = _settings, Authors = _repositories.Authors.Get() };

            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View(new ViewModelBase() {Settings = _settings});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { Settings = _settings, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
