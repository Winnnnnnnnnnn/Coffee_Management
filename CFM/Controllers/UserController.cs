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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
                name = "<a class='btn btn-link text-decoration-none' href='/User/Edit/" + u.Id + "'>" + u.Name + " </ a > ",
                role = u.getRoleName(),
                email = u.Email,
                phone = "" + u.Phone,
                action = "<form action='/User/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + u.Id + "' data-id='" + u.Id + "'/> <button type='button' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });

            return Json(new
            {
                data = data,
            });
        }

        public IActionResult Get(int id)
        {
            User user = userRepository.GetUserByID(id);
            ViewBag.IsActive = "user";
            return Json(user);
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
                    if (userRepository.IsEmailExists(user.Email))
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại");
                    }

                    if (userRepository.IsPhoneExists(user.Phone))
                    {
                        ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại");
                    }

                    if (userRepository.IsEmailExists(user.Email) || userRepository.IsPhoneExists(user.Phone))
                    {
                        return View(user);
                    }
                    user.Password = Helper.HashPassword(user.Password);
                    userRepository.InsertUser(user);
                    var dbContext = new Coffee_ManagementContext();
                    User auth = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = auth.Id,
                        Action = "Đã tạo",
                        Object = "Tài khoản",
                        ObjectId = user.Id,
                    });
                    dbContext.SaveChanges();
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
                var existingUser = userRepository.GetUserByID(id);

                // Kiểm tra xem người dùng đã thay đổi số điện thoại hoặc email hay không
                if (existingUser.Email != user.Email && userRepository.IsEmailExists(user.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(user);
                }
                if (existingUser.Phone != user.Phone && userRepository.IsPhoneExists(user.Phone))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại");
                    return View(user);
                }

                // Cập nhật thông tin người dùng
                user.Password = existingUser.Password;
                userRepository.UpdateUser(user);
                user.Password = userRepository.GetUserByID(id).Password;
                // Tiến hành cập nhật thông tin vai trò
                userRepository.UpdateUser(user);
                User auth = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                var dbContext = new Coffee_ManagementContext();
                LogDAO dao = new LogDAO();
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = auth.Id,
                    Action = "Đã cập nhật",
                    Object = "Tài khoản",
                    ObjectId = user.Id,
                });
                dbContext.SaveChanges();
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
                    User auth = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    var dbContext = new Coffee_ManagementContext();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = auth.Id,
                        Action = "Đã xóa",
                        Object = "Tài khoản",
                        ObjectId = id,
                    });
                    dbContext.SaveChanges();
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
            return Helper.UserInfo(HttpContext);
        }
    }
}
