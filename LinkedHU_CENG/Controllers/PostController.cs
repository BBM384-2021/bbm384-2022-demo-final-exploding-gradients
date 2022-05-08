using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
namespace LinkedHU_CENG.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PostController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PartialView(_db.Posts.ToList());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                post.UserId = userId;
                var user = _db.Users.Find(userId);
                post.UserName = user.Name + " " + user.Surname;
                post.UserProfilePicture = user.ProfilePicturePath;

                if(post.PostImage != null)
                {
                    string uniqueFileName = UploadedFile(post);
                    string[] name = uniqueFileName.Split(".");

                    if (name[1] == "mp4")
                    {
                        post.PostVideoPath = uniqueFileName;
                    }
                    else
                    {
                        post.PostImagePath = uniqueFileName;
                    }
                }
                
                _db.Posts.Add(post);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");

            }
            return View(post);

        }

        // GET
        public ActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _db.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST
        [HttpPost]
        public ActionResult Update(Post post)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(post);
                if (uniqueFileName != null)
                {
                    string[] name = uniqueFileName.Split(".");

                    if (name[1] == "mp4")
                    {
                        post.PostVideoPath = uniqueFileName;
                    }
                    else
                    {
                        post.PostImagePath = uniqueFileName;
                    }
                }

                _db.Posts.Update(post);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some error occured!");
            }
            return View(post);
        }

        public ActionResult Delete(int? id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(post);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private string UploadedFile(Post post)
        {
            string uniqueFileName = null;

            if (post.PostImage!= null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "postImages");


                if (post.PostImagePath != null && System.IO.File.Exists(Path.Combine(uploadsFolder, post.PostImagePath)))
                {
                    System.IO.File.Delete(Path.Combine(uploadsFolder, post.PostImagePath));
                    post.PostImagePath = null;
                }

                if (post.PostVideoPath != null && System.IO.File.Exists(Path.Combine(uploadsFolder, post.PostVideoPath)))
                {
                    System.IO.File.Delete(Path.Combine(uploadsFolder, post.PostVideoPath));
                    post.PostVideoPath = null;
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + post.PostImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    post.PostImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}