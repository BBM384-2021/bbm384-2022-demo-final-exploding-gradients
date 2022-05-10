using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    public class CommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CommentViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Comment> mc = await _db.Comments.ToListAsync();
            ViewData["SessionUserId"] = HttpContext.Session.GetInt32("UserID");
            return View(mc);
        }
    }
}
