using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;


namespace LinkedHU_CENG.Controllers
{

    public class AdministratorController : Controller
    {

        private readonly ApplicationDbContext db;

        public AdministratorController(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Administrator");
            }
        }


        public IActionResult Login(Administrator administrator)
        {

            var info = db.Administrators.FirstOrDefault(u => u.UserName.Equals(administrator.UserName) && u.Password.Equals(administrator.Password));
            if (info != null)
            {
                HttpContext.Session.SetString("UserName", info.UserName);

                return RedirectToAction("Index", "Administrator");
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Administrator");
            }
            else
            {
                return RedirectToAction("Index", "Administrator");
            }
        }

        public IActionResult VerifyAccounts()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                HttpContext.Session.Clear();

                return RedirectToAction("VerifyAccounts", "Administrator");
            }
            else
            {
                return View("VerifyAccounts");
            }
        }

        public IActionResult VerifyAnAccount(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                HttpContext.Session.Clear();

                return RedirectToAction("VerifyAccounts", "Administrator");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var unregisteredUser = db.UnregisteredUsers.Find(id);
                    User user = new User() { Name = unregisteredUser.Name, Surname = unregisteredUser.Surname, Password = unregisteredUser.Password, Email = unregisteredUser.Email, Role = unregisteredUser.Role, BirthDate = unregisteredUser.BirthDate, PhoneNum = unregisteredUser.PhoneNum };
     
                    db.UnregisteredUsers.Remove(unregisteredUser);

                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("VerifyAccounts", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                }
                return RedirectToAction("VerifyAccounts", "Administrator");

            }
        }
    }
}
