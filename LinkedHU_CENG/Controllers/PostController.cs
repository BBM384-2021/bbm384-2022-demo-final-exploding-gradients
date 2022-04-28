using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
namespace LinkedHU_CENG.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostController(ApplicationDbContext db)
        {
            _db = db;
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

    }
}