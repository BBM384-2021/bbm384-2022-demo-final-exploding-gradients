using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "ForgetPasswordViewComponent")] //Solution
    public class ForgetPasswordViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public ForgetPasswordViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<ForgetPassword> mc = await _db.ForgetPasswords.ToListAsync();
            return View(mc);
        }
    }
}
