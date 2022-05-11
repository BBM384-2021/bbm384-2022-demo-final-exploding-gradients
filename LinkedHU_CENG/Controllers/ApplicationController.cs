using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;

namespace LinkedHU_CENG.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApplicationController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = hostEnvironment;
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
        public IActionResult CreateApplication(int id)
        {
            var ResumeFile = HttpContext.Request.Form["ResumeFile"];
            if (ModelState.IsValid)
            {
                IFormFile a = ResumeFile;
                var advertisement = db.Advertisements.Find(id);
                Application newApplication = new Application();
                newApplication.AdvertisementId = advertisement.AdvertisementId;
                newApplication.Company = advertisement.Company;
                newApplication.AdvertisementTitle = advertisement.Title;
                newApplication.Resume = (IFormFile) ResumeFile;

                string uniqueResumeFileName = UploadedResume(newApplication);
                newApplication.ResumePath = uniqueResumeFileName;

                var userId = HttpContext.Session.GetInt32("UserID");
                newApplication.UserId = userId;
                var user = db.Users.Find(userId);
                newApplication.UserName = user.Name + " " + user.Surname;

                db.Applications.Add(newApplication);
                db.SaveChanges();
                return RedirectToAction("Index", "Advertisement");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return RedirectToAction("Index", "Advertisement");
        }

        private string UploadedResume(Application application)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "resumes");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + application.Resume.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                application.Resume.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
