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
            List<Application> applications = db.Applications.ToList();
            ViewData["Application"] = applications;
            return View();
        }

        public IActionResult Create(int id)
        {
            var advertisement = db.Advertisements.Find(id);
            ViewData["Advertisement"] = advertisement;
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
                return RedirectToAction("Create", "Advertisement");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View(application);
        }
    }
}
