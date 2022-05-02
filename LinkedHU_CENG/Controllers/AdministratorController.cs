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
            if (HttpContext.Session.GetString("Admin_UserName") == null)
            {
                return RedirectToAction("Login", "Administrator");
            }
            else
            {
                return View();
            }
            
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin_UserName") == null)
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
                HttpContext.Session.SetString("Admin_UserName", info.UserName);

                return RedirectToAction("Index", "Administrator");
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                //HttpContext.Session.Clear();
                HttpContext.Session.Remove("Admin_UserName");
                return RedirectToAction("Index", "Administrator");
            }
            else
            {
                return RedirectToAction("Index", "Administrator");
            }
        }

        public IActionResult VerifyAccounts()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");
        }

        public IActionResult VerifyAnAccount(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
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
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {
                
                return RedirectToAction("Index", "Administrator");

            }
        }

        [HttpGet]
        public IActionResult ReportedUser()
        {

            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");

        }




        [HttpGet]
        public IActionResult BannedUser()
        {

            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                List<BannedUser> bannedUsers = db.BannedUsers.ToList();
                ViewData["bannedUsers"] = bannedUsers;

                
                List<User> users = new List<User>();
                ViewData["users"] = users;
                return View();
            }
            return RedirectToAction("Index", "Administrator");

        }



        [HttpPost]
        public IActionResult BannedUserSearch()
        {
            string name = HttpContext.Request.Form["SearchName"];
            string surname = HttpContext.Request.Form["SearchSurname"];
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                List<BannedUser> bannedUsers = db.BannedUsers.ToList();
                List<User> users = db.Users.Where(m=> m.Name.Equals(name) && m.Surname.Equals(surname)).ToList();
                
                ViewData["users"] = users;
                ViewData["bannedUsers"] = bannedUsers;
                return View("BannedUser");
            }
            return RedirectToAction("Index", "Administrator");

        }




        public IActionResult BannedUserAccept(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.Users.Find(id);
                    BannedUser bannedUser = new BannedUser();
                    bannedUser.UserId = user.UserId;
                    bannedUser.Name = user.Name;
                    bannedUser.Surname = user.Surname;
                    bannedUser.Email = user.Email;

                    db.BannedUsers.Add(bannedUser);
                    db.SaveChanges();

                    return RedirectToAction("BannedUser", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {

                return RedirectToAction("Index", "Administrator");

            }
        }



        public IActionResult BannedUserRevert(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.BannedUsers.Find(id);
                    db.BannedUsers.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("BannedUser", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {

                return RedirectToAction("Index", "Administrator");

            }
        }
    }
}
