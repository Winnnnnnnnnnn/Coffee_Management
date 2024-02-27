using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.Repository;
using MyLibrary.DataAccess;

namespace CFM.Controllers
{
    public class UserController : Controller
    {
        IUserRepository userRepository = null;
        public UserController() => userRepository = new UserRepository();

        public ActionResult Index()
        {
            var userList = userRepository.GetUsers();
            return View("~/Views/User/Index.cshtml", userList);
        }


        public ActionResult Create() => View("~/Views/User/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userRepository.InsertUser(user);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(user);
            }
        }


        public object Edit(int? id)
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
            return Json(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid)
                {
                    return View("_ModalEditUser", user);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    var existingUser = userRepository.GetUserByID(id);
                    user.Password = existingUser.Password;
                }

                // Tiến hành cập nhật thông tin người dùng
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
