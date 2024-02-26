using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.DataAccess;
using MyLibrary.Repository;

namespace CoffeeManagement.Controllers
{
    public class PasswordController : Controller
    {
        private readonly Coffee_ManagementContext _db;
        private readonly IUserRepository _userRepository;

        public PasswordController(Coffee_ManagementContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(100000, 1000000);
            return otp.ToString();
        }

        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    return BadRequest("Invalid email address");
                }
                var user = _db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    ViewData["Null"] = "Your email is not registered!!";
                    return View("ForgotPassword");
                }

                string otp = GenerateOTP();

                string fromAddress = "phamcongto1512@gmail.com";
                string fromPassword = "palmvlgqurncodis";

                string toAddress = email;
                string subject = "OTP CODE";
                string body = $"Your OTP code is: {otp}";

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress, fromPassword),
                    Timeout = 20000
                };

                using (MailMessage message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);
                }

                TempData["OTP"] = otp;
                TempData["Email"] = email;

                return RedirectToAction("VerifyOtp", "Password");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ForgotPassword", "Password");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult VerifyOtp()
        {
            return View("VerifyOtp");
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otpInput, string generatedOTP)
        {

            generatedOTP = TempData["OTP"] as string;
            TempData.Keep("OTP");
            if (otpInput == generatedOTP)
            {
                return RedirectToAction("UpdatePassword", "Password");
            }
            else
            {
                ViewData["OTP"] = "Wrong otp code! Please enter again!";
                return View("VerifyOtp");

            }
        }

        public IActionResult UpdatePassword(string email)
        {
            email = TempData["Email"] as string;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return NotFound(); // Or handle accordingly
            ViewData["Message"] = user.Email;
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePassword(string email, string password, string confirmPassword)
        {

            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            ViewData["Message"] = user.Email;

            if (password != confirmPassword)
            {
                ViewData["Password"] = "Password and confirm password do not match!";
                return View("UpdatePassword");
            }
            else
            {

                // Hash the password using MD5
                string hashedPassword;
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    hashedPassword = sb.ToString();
                }

                user.Password = hashedPassword;
                _db.SaveChanges();
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
