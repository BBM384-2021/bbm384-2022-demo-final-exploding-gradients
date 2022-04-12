using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LinkedHU_CENG.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProfileController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {


                if (HttpContext.Session.GetString("UserID") != null)
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
    }
}
