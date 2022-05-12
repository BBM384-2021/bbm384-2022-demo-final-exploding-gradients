using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using LinkedHU_CENG.Models.ViewModels;

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
            var userId = HttpContext.Session.GetInt32("UserID");
            List<Application> applications = db.Applications.Where(a => a.UserId == userId).ToList();
            ViewData["Application"] = applications;
            ViewData["SessionUserId"] = userId;
            return View();
        }

        public IActionResult Create(AdvertisementViewModel viewModel)
        {
            viewModel.Advertisement = db.Advertisements.Find(viewModel.AdvertisementId);
            viewModel.certificate = new Certificate();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateApplication(AdvertisementViewModel viewModel)
        {
            Application application = viewModel.Application;
            int advertisementId = viewModel.AdvertisementId;

            var userId = HttpContext.Session.GetInt32("UserID");
            application.UserId = userId;
            var user = db.Users.Find(userId);
            application.UserName = user.Name + " " + user.Surname;
            application.PhoneNum = user.PhoneNum;
            application.UserAbout = user.About;
            application.UserLocation = user.Location;
            application.Email = user.Email;
            application.AdvertisementId = advertisementId;
            viewModel.Advertisement = db.Advertisements.Find(viewModel.AdvertisementId);
            application.AdvertisementTitle = viewModel.Advertisement.Title;
            application.Company = viewModel.Advertisement.Company;

            viewModel.certificate.ApplicationId = application.ApplicationId;
            viewModel.certificate.UserId = userId;

            if (application.Resume != null)
            {
                string uniqueFileName = UploadedResume(application);
                application.ResumePath = uniqueFileName;
            }

            if (viewModel.certificate.File != null)
            {
                string uniqueFileName = UploadedCertificate(viewModel.certificate);
                viewModel.certificate.FilePath = uniqueFileName;
            }

            db.Applications.Add(application);
            db.Certificates.Add(viewModel.certificate);
            db.SaveChanges();
            return RedirectToAction("Index", "Advertisement");
        }

        [HttpGet]
        public IActionResult ViewApplication(Application application)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            application = db.Applications.Find(application.ApplicationId);
            ViewData["Application"] = application;
            return View();
        }

        private string UploadedResume(Application application)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Resumes");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + application.Resume.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                application.Resume.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        private string UploadedCertificate(Certificate certificate)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Certificates");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + certificate.File.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                certificate.File.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
