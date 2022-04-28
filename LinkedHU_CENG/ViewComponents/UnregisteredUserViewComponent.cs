using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "UnregisteredViewComponent")] //Solution
    public class UnregisteredViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public UnregisteredViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<UnregisteredUser> mc = await _db.UnregisteredUsers.ToListAsync();
            return View(mc);
        }
    }
}
