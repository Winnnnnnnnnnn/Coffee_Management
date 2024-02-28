using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;

namespace CFM.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Coffee_ManagementContext _db;
        private readonly IUserRepository _userRepository;

        public ProfileController(Coffee_ManagementContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            int useId = (int)TempData["Id"];
            System.Console.WriteLine(useId);
            string passMD5;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(oldPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                passMD5 = sb.ToString();
            }

            oldPassword = passMD5;
            var user = _db.Users.FirstOrDefault(u => u.Password == oldPassword && u.Id == useId);
            System.Console.WriteLine(oldPassword);
            System.Console.WriteLine(useId);




            if (newPassword != confirmPassword)
            {
                ViewData["Password"] = "Password and confirm password do not match!";
                return View("Index", "Profile");
            }
            else
            {

                // Hash the password using MD5
                string hashedPassword;
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(newPassword);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    hashedPassword = sb.ToString();
                }
                Console.WriteLine(hashedPassword);

                user.Password = hashedPassword;
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}