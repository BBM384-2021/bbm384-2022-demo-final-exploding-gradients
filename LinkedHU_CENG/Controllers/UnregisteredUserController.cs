using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;

namespace LinkedHU_CENG.Controllers
{
    public class UnregisteredUserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UnregisteredUserController(ApplicationDbContext context)
        {
            this.db = context;
        }


        public IActionResult Index()
        {
            return View(db.UnregisteredUsers.ToList());
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Register(UnregisteredUser usr)
        {
            if (ModelState.IsValid)
            {
                db.UnregisteredUsers.Add(usr);
                db.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var post = db.UnregisteredUsers.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            db.UnregisteredUsers.Remove(post);
            db.SaveChanges();

            return RedirectToAction("VerifyAccounts", "Administrator");
        }
    }
}
