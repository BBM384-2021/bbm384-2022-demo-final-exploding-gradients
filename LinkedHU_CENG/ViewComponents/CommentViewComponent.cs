using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "CommentViewComponent")] //Solution
    public class CommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CommentViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            IEnumerable<Comment> mc = await _db.Comments.Where(t => t.PostId == postId).ToListAsync();
            ViewData["SessionUserId"] = HttpContext.Session.GetInt32("UserID");
            return View(mc);
        }
    }
}
