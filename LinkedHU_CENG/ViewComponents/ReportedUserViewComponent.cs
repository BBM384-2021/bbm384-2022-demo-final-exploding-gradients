
using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "ReportedUserViewComponent")] //Solution

    public class ReportedUserViewComponent : ViewComponent
    { 
        private readonly ApplicationDbContext _db;

        public ReportedUserViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<Report> mc = await _db.Reports.ToListAsync();
            ViewData["SessionUserName"] = HttpContext.Session.GetInt32("UserName");
            return View(mc);
        }
    }

}

