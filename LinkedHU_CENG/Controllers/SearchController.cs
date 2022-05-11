using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.SqlClient;

namespace LinkedHU_CENG.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db;

        public SearchController(ApplicationDbContext context)
        {
            this.db = context;
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                string word = HttpContext.Request.Form["word"];

                System.Diagnostics.Debug.WriteLine(word);
                char[] charsToTrim = { '*', ' ', '\'' };
                string[] wordSearch = word.Trim(charsToTrim).Split(" ");

                string nameSurnameCommand = $"SELECT * FROM \"Users\" WHERE \"Name\" ILIKE '%{word}%'";
                var nameSurnameList = db.Users.FromSqlRaw(nameSurnameCommand).ToList();

                string nameCommand = $"SELECT * FROM \"Users\" WHERE \"Name\" ILIKE '%{wordSearch[0]}%'";
                var nameList = db.Users.FromSqlRaw(nameCommand).ToList();
                var surnameList = new List<User>();

                if (wordSearch.Length > 1)
                {
                    string surnameCommand = $"SELECT * FROM \"Users\" WHERE \"Surname\" ILIKE '%{wordSearch[1]}%'";
                    surnameList = db.Users.FromSqlRaw(surnameCommand).ToList();
                }

                foreach (var usrNameSurname in nameSurnameList)
                {
                    nameList.Remove(usrNameSurname);
                    surnameList.Remove(usrNameSurname);
                }
                foreach (var usrNameSurname in nameList)
                {
                    surnameList.Remove(usrNameSurname);
                }

                ViewData["NameSurname"] = nameSurnameList;
                ViewData["Name"] = nameList;
                ViewData["Surname"] = surnameList;

                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ViewProfile(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                System.Diagnostics.Debug.WriteLine(id.ToString());
                var user = db.Users.Find(id);
                List<Post> posts = db.Posts.OrderByDescending(x => x.CreatedAt).Where(x => x.UserId == id).ToList();
                ViewData["SessionUserId"] = HttpContext.Session.GetInt32("UserID");
                ViewData["User"] = user;
                ViewData["Posts"] = posts;

                return View("Profile");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
