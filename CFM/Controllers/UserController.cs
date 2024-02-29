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
            Coffee_ManagementContext cft = new Coffee_ManagementContext();
            var roles = cft.Roles.ToList();
            ViewBag.Roles = roles;
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
                name = "<button type='button' class='btn btn-link text-decoration-none btn-update-user' data-id='" + u.Id + "'>" + u.Name + "</button>",
                email = u.Email,
                phone = u.Phone,
                role_id = u.RoleId,
                action = "<form action='/User/Delete' method='post' class='save-form'><input type='hidden' name='choices[]' value='" + u.Id + "' data-id='" + u.Id + "'/> <button type='submit' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });

            return Json(new { data = data });
        }


        // public ActionResult Create() => View("~/Views/Includes/_ModalCreateUser.cshtml");

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Create(User user)
        // {
        //     try
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             userRepository.InsertUser(user);
        //             // return RedirectToAction(nameof(Index));
        //             return Json(new { success = true }); // Trả về kết quả thành công
        //         }
        //         else
        //         {
        //             return View("Index", user);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         ViewBag.Message = ex.Message;
        //         return View("Index", user);
        //     }
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Xử lý logic tạo mới User
                    return Json(new { success = true }); // Trả về kết quả thành công
                }
                else
                {
                    // Nếu ModelState không hợp lệ, trả về lỗi để hiển thị trong modal
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi để hiển thị trong modal
                return Json(new { success = false, errorMessage = ex.Message });
            }
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
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                // Lấy thông tin người dùng cần cập nhật từ repository
                var existingUser = userRepository.GetUserByID(id);
                if (existingUser == null)
                {
                    return Json(new { success = false, errorMessage = "Không tìm thấy người dùng." });
                }

                // Cập nhật thông tin từ dữ liệu đầu vào vào người dùng hiện tại
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.RoleId = user.RoleId;

                // Nếu mật khẩu được cung cấp, cập nhật lại mật khẩu
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = user.Password;
                }

                // Tiến hành cập nhật thông tin người dùng
                userRepository.UpdateUser(existingUser);

                // Trả về kết quả thành công
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }

    }
}
