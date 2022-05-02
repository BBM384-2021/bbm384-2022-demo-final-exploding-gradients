using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LinkedHU_CENG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()

        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return View("WelcomePage");
            }
            else
            {
                return View("LoggedIn"); ;
            }
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