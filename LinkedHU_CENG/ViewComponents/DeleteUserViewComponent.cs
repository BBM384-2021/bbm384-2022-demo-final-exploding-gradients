using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "DeleteUserViewComponent")] //Solution
    public class DeleteUserViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public DeleteUserViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<DeleteRequest> mc = await _db.DeleteRequests.ToListAsync();
            return View(mc);
        }
    }
}
