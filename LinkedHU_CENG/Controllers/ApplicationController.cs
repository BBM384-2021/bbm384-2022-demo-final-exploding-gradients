using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;

namespace LinkedHU_CENG.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext db;
        public ApplicationController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Application application)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                application.UserId = userId;
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index", "Application");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View(application);
        }
    }
}
