using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "AdvertisementViewComponent")]
    public class AdvertisementViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext db;

        public AdvertisementViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<Advertisement> mc = await db.Advertisements.ToListAsync();
            ViewData["SessionUserId"] = HttpContext.Session.GetInt32("UserID");
            return View(mc);
        }
    }
}
