using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
namespace LinkedHU_CENG.Controllers

{
    public class AdvertisementController : Controller
    {
        private readonly ApplicationDbContext db;
        public AdvertisementController(ApplicationDbContext db)
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
        public IActionResult Create(Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                advertisement.UserId = userId;
                var user = db.Users.Find(userId);
                advertisement.UserName = user.Name + " " + user.Surname;

                db.Advertisements.Add(advertisement);
                db.SaveChanges();
                return RedirectToAction("Index", "Advertisement");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View(advertisement);
        }

        public ActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var advertisement = db.Advertisements.Find(id);

            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }


        [HttpPost]
        public ActionResult Update(Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                db.Advertisements.Update(advertisement);
                db.SaveChanges();
                return RedirectToAction("Update", "Advertisement");
            }
            else
            {
                ModelState.AddModelError("", "Some error occured!");
            }
            return View(advertisement);
        }

        public ActionResult Delete(int? id)
        {
            var advertisement = db.Advertisements.Find(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            db.Advertisements.Remove(advertisement);
            db.SaveChanges();

            return RedirectToAction("Index", "Advertisement");
        }
    }
}
