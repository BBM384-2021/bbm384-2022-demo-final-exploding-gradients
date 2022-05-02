﻿using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;

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




        [HttpGet]
        public IActionResult BannedUser()
        {

            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                List<BannedUser> bannedUsers = db.BannedUsers.ToList();
                ViewData["bannedUsers"] = bannedUsers;
                List<User> users = new List<User>();
                ViewData["users"] = users;

                return View();
            }
            return RedirectToAction("Index", "Administrator");

        }

        [HttpPost]
        public IActionResult BannedUserSearch()
        {
            string name = HttpContext.Request.Form["SearchName"];
            string surname = HttpContext.Request.Form["SearchSurname"];
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                List<BannedUser> bannedUsers = db.BannedUsers.ToList();
                List<User> users = db.Users.Where(m=> m.Name.Equals(name) && m.Surname.Equals(surname)).ToList();
                
                ViewData["users"] = users;
                ViewData["bannedUsers"] = bannedUsers;

                return View("BannedUser");
            }
            return RedirectToAction("Index", "Administrator");
        }

        public IActionResult BannedUserAccept(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.Users.Find(id);
                    BannedUser bannedUser = new BannedUser();
                    bannedUser.UserId = user.UserId;
                    bannedUser.Name = user.Name;
                    bannedUser.Surname = user.Surname;
                    bannedUser.Email = user.Email;

                    db.BannedUsers.Add(bannedUser);
                    db.SaveChanges();

                    return RedirectToAction("BannedUser", "Administrator");
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

        public IActionResult BannedUserRevert(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.BannedUsers.Find(id);
                    db.BannedUsers.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("BannedUser", "Administrator");
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




        public IActionResult DeleteUser()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");
        }

        public IActionResult DeleteUserAccept(int id) // kullanıcının post ve announcementlarını da siliyor
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



        
        public IActionResult ForgetPassword()
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Administrator");
        }

        public IActionResult DeleteForgetPasswordRequest(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var request = db.ForgetPasswords.Where(m=>m.ID.Equals(id)).FirstOrDefault();
                    db.ForgetPasswords.Remove(request);
                    db.SaveChanges();

                    return RedirectToAction("ForgetPassword", "Administrator");
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

        public IActionResult ForgetPasswordAccept(int id)
        {
            if (HttpContext.Session.GetString("Admin_UserName") != null)
            {
                if (ModelState.IsValid)
                {
                    var request = db.ForgetPasswords.Where(m => m.ID.Equals(id)).FirstOrDefault();
                    var user = db.Users.Where(m => m.Email.Equals(request.Email)).FirstOrDefault();
                    RandomNumberGenerator generator = new RandomNumberGenerator();
                    string pass = generator.RandomPassword();  // kullanıcıya gönderilecek parolayı yaratıyor, aşağıda fonksiyonu mevcut, ortak bir dosyaya kaydedilse daha iyi olur

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add("mobilmurat4@gmail.com"); // buraya maili göndereceğimiz adres user.Email gelecek
                    mail.From = new MailAddress("explodinggradient2022@gmail.com", "Email head", System.Text.Encoding.UTF8);
                    mail.Subject = "Password Request for" + user.Name;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = "\nYour new password is : " + pass; // şuraya güzel bir yazı yazılabilir
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential("explodinggradient2022@gmail.com", "Bbm3842022");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    try
                    {
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        RedirectToAction("Index", "Administrator");
                    }

                    user.Password = Encrypt(pass); // parolayı veritabanında da güncelliyor
                    db.Users.Update(user);

                    db.ForgetPasswords.Remove(request);
                    db.SaveChanges();


                    return RedirectToAction("ForgetPassword", "Administrator");
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


        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }



    }


    public class RandomNumberGenerator   // bu sınıfı bi düzgünlestirmek lazım
    {
        // Generate a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)  
        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }


}
