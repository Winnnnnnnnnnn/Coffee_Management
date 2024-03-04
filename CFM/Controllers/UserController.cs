using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.Repository;
using MyLibrary.DataAccess;
using MyMVC.Models.Authentication;
using CoffeeManagement.Controllers;

namespace CFM.Controllers
{
    [Authentication]
    public class UserController : Controller
    {
        IUserRepository userRepository = null;
        public UserController() => userRepository = new UserRepository();

        public ActionResult Index()
        {
            ViewBag.IsActive = "user";
            return View();
        }

        public IActionResult Load()
        {
            var users = userRepository.GetUsers();
            var data = users.Select(u => new
            {
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + u.Id + "'>",
                id = u.Id,
                name = "<a class='btn btn-link text-decoration-none' href='/User/Edit/" + u.Id + "'>" + u.Name + " </ a > ",
                role = u.Role,
                email = u.Email,
                phone = "" + u.Phone,
                action = "<form action='/User/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + u.Id + "' data-id='" + u.Id + "'/> <button type='button' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });
            return Json(new { data = data });
        }

        public IActionResult Create()
        {
            ViewBag.IsActive = "user";
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(user);
                }
                else
                {
                    user.Password = PasswordHelper.HashPassword(user.Password);
                    System.Console.WriteLine(user.Password);
                    userRepository.InsertUser(user);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(user);
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = userRepository.GetUserByID(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            System.Console.WriteLine(user.Password);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                user.Password = userRepository.GetUserByID(id).Password;
                // Tiến hành cập nhật thông tin vai trò
                userRepository.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                ViewBag.Message = ex.Message;
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (userRepository.GetUserByID(int.Parse(id)) == null)
            {
                return NotFound();
            }
            userRepository.DeleteUser(int.Parse(id));
            return RedirectToAction("Index");
        }
    }
}
