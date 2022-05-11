using LinkedHU_CENG.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace LinkedHU_CENG.Controllers
{

    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            return View(db.Users.ToList());
        }



        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        public IActionResult Login(User usr)
        {
            
            var info = db.Users.FirstOrDefault(u => (u.Email.Equals(usr.Email) || u.SecondEmail.Equals(usr.Email)) && u.Password.Equals(Encrypt(usr.Password)));
            if (info != null)
            {
                HttpContext.Session.SetInt32("UserID", info.UserId);
                HttpContext.Session.SetString("Email", info.Email);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                //HttpContext.Session.Clear();
                HttpContext.Session.Remove("UserID");
                HttpContext.Session.Remove("Email");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                var deleteUser = db.Users.Find(id);
                DeleteRequest request = new DeleteRequest();
                request.UserId = id;
                request.Name = deleteUser.Name;
                request.Email = deleteUser.Email;
                request.Surname = deleteUser.Surname;
                request.Role = deleteUser.Role;
                db.DeleteRequests.Add(request);
                db.SaveChanges();

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPassword forgetUser)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => (m.Email.Equals(forgetUser.Email) || m.SecondEmail.Equals(forgetUser.Email))).FirstOrDefault();
                if (user != null)
                {
                    forgetUser.Name = user.Name;
                    forgetUser.Surname = user.Surname;
                    db.ForgetPasswords.Add(forgetUser);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View();
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

        public static string GetAllColumns()
        {
            return "UserId,Name,Surname,Email,PhoneNum,Role,Birthdate,About Me,Location";
        }
        private string Decrypt(string cipherText) // bu fonksiyonu nerede kullanıcaz bilmiyorum ama kaybetmemek adına buraya ekledim
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
