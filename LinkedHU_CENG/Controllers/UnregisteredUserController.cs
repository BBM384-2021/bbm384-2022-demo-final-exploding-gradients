using Microsoft.AspNetCore.Mvc;
using LinkedHU_CENG.Models;
using System.Text;
using System.Security.Cryptography;

namespace LinkedHU_CENG.Controllers
{
    public class UnregisteredUserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UnregisteredUserController(ApplicationDbContext context)
        {
            this.db = context;
        }


        public IActionResult Index()
        {
            return View(db.UnregisteredUsers.ToList());
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Register(UnregisteredUser usr)
        {
            if (ModelState.IsValid)
            {
                usr.Password = Encrypt(usr.Password);
                db.UnregisteredUsers.Add(usr);
                db.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var post = db.UnregisteredUsers.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            db.UnregisteredUsers.Remove(post);
            db.SaveChanges();

            return RedirectToAction("VerifyAccounts", "Administrator");
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
}
