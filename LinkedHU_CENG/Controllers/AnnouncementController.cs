using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
namespace LinkedHU_CENG.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AnnouncementController(ApplicationDbContext db)
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
        public IActionResult Create(Announcement announcement)

        {
            if (ModelState.IsValid)
            {
                _db.Announcements.Add(announcement);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View(announcement);

        }

        // GET
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var announcement = _db.Announcements.Find(id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST
        [HttpPost]
        public ActionResult Edit(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                _db.Announcements.Update(announcement);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some error occured!");
            }
            return View(announcement);
        }

        public ActionResult Delete(int? id)
        {
            var announcement = _db.Announcements.Find(id);
            if (announcement == null)
            {
                return NotFound();
            }
            _db.Announcements.Remove(announcement);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
