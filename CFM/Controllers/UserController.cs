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
    // [Authentication]
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
            int recordsTotal = users.Count();
            var data = users.Select(u => new
            {
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + u.Id + "'>",
                id = u.Id,
                name = "<a class='btn btn-link text-decoration-none' href='/User/Edit/" + u.Id + "'>" + u.Name + " </ a > ",
                role = u.getRoleName(),
                email = u.Email,
                phone = "" + u.Phone,
                action = "<form action='/User/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + u.Id + "' data-id='" + u.Id + "'/> <button type='button' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });
            var language = new
            {
                sProcessing = "Đang xử lý...",
                sLengthMenu = "_MENU_ dòng /  trang",
                emptyTable = "Không có dữ liệu",
                sZeroRecords = "Không có kết quả nào được tìm thấy",
                sInfo = "Hiển thị từ _START_ đến _END_ của _TOTAL_ mục",
                sInfoEmpty = "Hiển thị từ 0 đến 0 của 0 mục",
                sInfoFiltered = "(đã lọc từ _MAX_ mục)",
                sInfoPostFix = "",
                sSearch = "Tìm kiếm:",
                sUrl = "",
                oPaginate = new
                {
                    sFirst = "&laquo;",
                    sLast = "&raquo;",
                    sNext = "&rsaquo;",
                    sPrevious = "&lsaquo;"
                }
            };

            return Json(new
            {
                recordsTotal = recordsTotal,
                data = data,
                language = language
            });
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
                    return View(user);
                }
                else
                {
                    user.Password = Helper.HashPassword(user.Password);
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
        public ActionResult Delete(int id)
        {
            object response = null;
            try
            {
                if (userRepository.GetUserByID(id) == null)
                {
                    response = new
                    {
                        title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                        status = "danger"
                    };
                    return Json(response);
                }
                else
                {
                    response = new
                    {
                        controller = "User",
                        title = "Đã xóa thành công.",
                        status = "success"
                    };
                    userRepository.DeleteUser(id);
                }
                return Json(response);
            }
            catch (System.Exception)
            {
                response = new
                {
                    title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                    status = "danger"
                };
                return Json(response);
            }
        }

        public User GetUserInfo()
        {
            System.Console.WriteLine(Helper.UserInfo(HttpContext).Name);
            return Helper.UserInfo(HttpContext);
        }
    }
}
