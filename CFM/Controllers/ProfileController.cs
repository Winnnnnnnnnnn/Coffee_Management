using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;
using Newtonsoft.Json;

namespace CFM.Controllers
{
    [Authentication]
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
        public IActionResult ChangePassword(string Password, string newPassword, string confirmPassword)
        {
            if (Password == null && newPassword == null && confirmPassword == null)
            {
                ViewData["allPassword"] = "Không thể để trống";
                return View("ChangePassword");
            }

            if (newPassword == null && confirmPassword == null)
            {
                ViewData["ncPassword"] = "Không thể để trống";
                return View("ChangePassword");
            }

            if (Password == null)
            {
                ViewData["oldPassword"] = "Không thể để trống";
                return View("ChangePassword");
            }
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 8)
            {
                ViewData["newPassword"] = "Mật khẩu phải có ít nhất 8 kí tự";
                return View("ChangePassword");
            }
            if (confirmPassword == null)
            {
                ViewData["confirmPassword"] = "Không thể để trống";
                return View("ChangePassword");
            }



            User user = Helper.UserInfo(HttpContext);
            string passMD5;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(Password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                passMD5 = sb.ToString();
            }

            Password = passMD5;
            User userUpdate = _db.Users.FirstOrDefault(u => u.Password == Password && u.Id == user.Id);



            if (newPassword != confirmPassword)
            {
                ViewData["invalidPassword"] = "Mật khẩu không trùng khớp";
                return View("ChangePassword");
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
                userUpdate.Password = hashedPassword;
                _db.SaveChanges();
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult ProfileUser()
        {
            HttpContext context = HttpContext;
            var session = context.Session;
            string key_access = "user";
            string jsonUser = session.GetString(key_access);
            User user = null;
            if (jsonUser != null)
            {
                user = JsonConvert.DeserializeObject<User>(jsonUser);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult ProfileUser(User user)
        {
            try
            {
                HttpContext context = HttpContext;
                var session = context.Session;
                string key_access = "user";
                string jsonUser = session.GetString(key_access);
                User userSession = null;
                if (jsonUser != null)
                {
                    userSession = JsonConvert.DeserializeObject<User>(jsonUser);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
                user.Id = userSession.Id;
                user.Password = userSession.Password;
                using (var db_context = new Coffee_ManagementContext())
                {
                    db_context.Users.Update(user);
                    db_context.SaveChanges();
                }
                return RedirectToAction("Index", "Login");
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                ViewBag.Message = ex.InnerException.Message;
                return View(user);
            }
        }

    }
}