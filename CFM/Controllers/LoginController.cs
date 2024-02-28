using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace ShopManagement.Controllers
{
    public class LoginController : Controller
    {
        IUserRepository userRepository = null;

        public LoginController() => userRepository = new UserRepository();

        public ActionResult Index()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var userList = userRepository.GetUsers();

            string hashedPassword = GetMd5Hash(user.Password);

            var authenticatedUser = userList.FirstOrDefault(u => u.Email == user.Email && u.Password == hashedPassword);

            if (authenticatedUser != null)
            {
                TempData["Id"] = authenticatedUser.Id;
                HttpContext.Session.SetInt32("AuthId", authenticatedUser.Id);
                HttpContext.Session.SetString("AuthName", authenticatedUser.Name);
                HttpContext.Session.SetString("AuthRole", userRepository.GetRole(authenticatedUser));

                return RedirectToAction("Index", "Home");
            }


            ViewData["Message"] = "Login Fail! Wrong email or password!";
            return View("~/Views/Login/Index.cshtml");
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}
