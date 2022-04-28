﻿using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.ViewComponents
{
    [ViewComponent(Name = "OwnedPostViewComponent")] //Solution
    public class OwnedPostViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public OwnedPostViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<Post> mc = await _db.Posts.ToListAsync();
            ViewData["SessionUserId"] = HttpContext.Session.GetInt32("UserID");
            return View(mc);
        }
    }
}
