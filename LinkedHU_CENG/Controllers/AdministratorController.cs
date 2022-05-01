﻿using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;


namespace LinkedHU_CENG.Controllers
{

    public class AdministratorController : Controller
    {

        private readonly ApplicationDbContext db;

        public AdministratorController(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin_UserName") == null)
            {
                return RedirectToAction("Login", "Administrator");
            }
            else
            {
                return View();
            }
            
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin_UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Administrator");
            }
        }


        public IActionResult Login(Administrator administrator)
        {

            var info = db.Administrators.FirstOrDefault(u => u.UserName.Equals(administrator.UserName) && u.Password.Equals(administrator.Password));
            if (info != null)
            {
                HttpContext.Session.SetString("Admin_UserName", info.UserName);

                return RedirectToAction("Index", "Administrator");
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                //HttpContext.Session.Clear();
                HttpContext.Session.Remove("Admin_UserName");
                return RedirectToAction("Index", "Administrator");
            }
            else
            {
                return RedirectToAction("Index", "Administrator");
            }
        }

        public IActionResult VerifyAccounts()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");
        }

        public IActionResult VerifyAnAccount(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var unregisteredUser = db.UnregisteredUsers.Find(id);
                    User user = new User() { Name = unregisteredUser.Name, Surname = unregisteredUser.Surname, Password = unregisteredUser.Password, Email = unregisteredUser.Email, Role = unregisteredUser.Role, BirthDate = unregisteredUser.BirthDate, PhoneNum = unregisteredUser.PhoneNum };

                    db.UnregisteredUsers.Remove(unregisteredUser);

                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("VerifyAccounts", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {
                
                return RedirectToAction("Index", "Administrator");

            }
        }

        [HttpGet]
        public IActionResult ReportedUser()
        {

            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");

        }





        public IActionResult DeleteUser()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");
        }


        public IActionResult DeleteUserAccept(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    List<Post> posts = db.Posts.Where(m => m.UserId == id).ToList<Post>();
                    List<Announcement> announcements = db.Announcements.Where(m => m.UserId == id).ToList<Announcement>();
                    foreach (var post in posts)
                    {
                        db.Posts.Remove(post);
                    }
                    foreach (var announcement in announcements)
                    {
                        db.Announcements.Remove(announcement);
                    }
                    var deleteRequest = db.DeleteRequests.Find(id);
                    db.DeleteRequests.Remove(deleteRequest);
                    var deleteUser = db.Users.Find(id);
                    db.Users.Remove(deleteUser);

                    if (HttpContext.Session.GetInt32("UserID") == id)
                    {
                        HttpContext.Session.Remove("UserID");
                        HttpContext.Session.Remove("Email");
                    }

                    db.SaveChanges();

                    return RedirectToAction("DeleteUser", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {

                return RedirectToAction("Index", "Administrator");

            }
        }




        public IActionResult DeleteUserRequest(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var deleteRequest = db.DeleteRequests.Find(id);
                    db.DeleteRequests.Remove(deleteRequest);
                    db.SaveChanges();

                    return RedirectToAction("DeleteUser", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Some Error Occured!");
                    return RedirectToAction("Index", "Administrator");

                }
            }
            else
            {

                return RedirectToAction("Index", "Administrator");

            }
        }



    }
}
