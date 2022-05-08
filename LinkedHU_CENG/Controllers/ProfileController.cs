using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkedHU_CENG.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.db = context;
            this.webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {


                if (HttpContext.Session.GetInt32("UserID") != null)
                {
                    var user = db.Users.FirstOrDefault(
                        u => u.UserId.Equals(HttpContext.Session.GetInt32("UserID")) && u.Email.Equals(HttpContext.Session.GetString("Email")));
                    //System.Diagnostics.Debug.WriteLine(user.Name);
                    //System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetInt32("UserID"));



                    ViewData["User"] = user;

                    //System.Diagnostics.Debug.WriteLine("hey");
                    //System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("UserID"));
                    //System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("Email"));
                    return View();
                }

                return RedirectToAction("Index", "Home");
           
        }

        public ActionResult Edit()
        {
            var user = db.Users.FirstOrDefault(u => u.UserId.Equals(HttpContext.Session.GetInt32("UserID")) && u.Email.Equals(HttpContext.Session.GetString("Email")));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(user);
                if(uniqueFileName != null)
                {
                    user.ProfilePicturePath = uniqueFileName;
                }

                var posts = db.Posts.Where(x => x.UserId == user.UserId).ToList();
                foreach(Post post in posts)
                {
                    post.UserProfilePicture = user.ProfilePicturePath;
                }

                var announcements = db.Announcements.Where(x => x.UserId == user.UserId).ToList();
                foreach (Announcement announcement in announcements)
                {
                    announcement.UserProfilePicture = user.ProfilePicturePath;
                }

                db.Users.Update(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                ModelState.AddModelError("", "Some error occured!");
            }


            return View();
        }

        private string UploadedFile(User user)
        {
            string uniqueFileName = null;

            if (user.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "profilePictures");

                if (user.ProfilePicturePath != null && System.IO.File.Exists(Path.Combine(uploadsFolder, user.ProfilePicturePath)))
                {
                    System.IO.File.Delete(Path.Combine(uploadsFolder, user.ProfilePicturePath));
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + user.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
