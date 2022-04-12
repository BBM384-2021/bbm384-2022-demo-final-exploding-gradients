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
                _db.Posts.Add(post);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");

            }
            return PartialView(post);

        }



    }
}