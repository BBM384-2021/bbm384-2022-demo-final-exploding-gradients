using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Http;



namespace LinkedHU_CENG.Controllers
{

    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(db.Users.ToList());
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
        public IActionResult Register(User usr)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(usr);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else 
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
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


        public IActionResult Login(User usr)
        {
            
            var info = db.Users.FirstOrDefault(u => u.Email.Equals(usr.Email) && u.Password.Equals(usr.Password));
            if (info != null)
            {
                HttpContext.Session.SetInt32("UserID", info.UserId);
                HttpContext.Session.SetString("Email", info.Email);
                return RedirectToAction("Index", "Profile");
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
