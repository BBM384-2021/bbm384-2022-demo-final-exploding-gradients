using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "PostViewComponent")] //Solution
    public class PostViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public PostViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
          
            IEnumerable<Post> mc = await _db.Posts.ToListAsync();
            return View(mc);
        }
    }
}
