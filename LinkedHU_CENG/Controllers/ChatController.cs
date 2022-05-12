using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkedHU_CENG.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChatController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var query = _db.Chats.Where(t => t.RecieverId== userId).Select(t => t.SenderId).Distinct()
                    .SelectMany(key => _db.Chats.Where(t => t.SenderId == key).OrderByDescending(t => t.CreatedAt).Take(1));
                IEnumerable<Chat> ResultChat = query;
                return View("Index", ResultChat);
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");

            }
            return View();

        }

        [HttpPost]
        public IActionResult Search()
        {
            string name = HttpContext.Request.Form["SearchName"];
            string surname = HttpContext.Request.Form["SearchSurname"];
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                List<User> users = _db.Users.Where(m => m.Name.Equals(name) && m.Surname.Equals(surname)).ToList();
                ViewData["users"] = users;
                return PartialView("ShowUsers");
            }
            return RedirectToAction("Index", "Chat");
        }
        public IActionResult Create()
        {
            return PartialView("_NewChat", "Chat");
        }

        [HttpPost]
        public IActionResult Create(Chat chat)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                chat.SenderId = userId;
                var user = _db.Users.Find(userId);
                chat.SenderUserName = user.Name + " " + user.Surname;

                _db.Chats.Add(chat);
                //_db.SaveChanges();
                return PartialView("_NewChat", "Chat");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");

            }
            return View(chat);

        }

    }
}
