using LinkedHU_CENG.Models;
using LinkedHU_CENG.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LinkedHU_CENG.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CommentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PartialView(_db.Comments.ToList());
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Create(PostCommentViewModel viewModel)
        {
            Comment comment = viewModel.comment;
            Post post = viewModel.post;

            if (comment.Content != null)
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                comment.UserId = userId;
                var user = _db.Users.Find(userId);
                comment.UserName = user.Name + " " + user.Surname;
                comment.UserProfilePicture = user.ProfilePicturePath;
                comment.PostId = post.PostId;

                _db.Comments.Add(comment);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View();
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(post);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
