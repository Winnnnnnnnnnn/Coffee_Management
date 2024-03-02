using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace CFM.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult ChangePassword()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (User.Identity.IsAuthenticated)
            {
                // Trả về view để đổi mật khẩu
                return View();
            }
            else
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Index", "Login");
            }
        }

        // Action để xử lý việc thay đổi mật khẩu
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            // Lấy ID của người dùng từ phiên đăng nhập
            string userId = User.Identity.Name;

            // Kiểm tra mật khẩu cũ có đúng không
            if (IsCurrentPasswordValid(userId, currentPassword))
            {
                // Nếu đúng, thay đổi mật khẩu mới
                if (ChangeUserPassword(userId, newPassword))
                {
                    // Trả về trang thông báo đổi mật khẩu thành công
                    return View("ChangePasswordSuccess");
                }
                else
                {
                    // Trả về trang thông báo lỗi khi đổi mật khẩu
                    ModelState.AddModelError("", "Đã có lỗi xảy ra khi đổi mật khẩu.");
                    return View();
                }
            }
            else
            {
                // Trả về trang thông báo lỗi khi mật khẩu cũ không đúng
                ModelState.AddModelError("", "Mật khẩu cũ không đúng.");
                return View();
            }
        }

        // Hàm kiểm tra mật khẩu cũ
        private bool IsCurrentPasswordValid(string userId, string currentPassword)
        {
            // Kết nối đến database và thực hiện kiểm tra
            using (SqlConnection connection = new SqlConnection("YourConnectionString"))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM User WHERE Id = @Id AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
                    command.Parameters.AddWithValue("@Password", currentPassword);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Hàm thay đổi mật khẩu mới
        private bool ChangeUserPassword(string userId, string newPassword)
        {
            // Kết nối đến database và thực hiện cập nhật mật khẩu mới
            using (SqlConnection connection = new SqlConnection("YourConnectionString"))
            {
                connection.Open();

                string query = "UPDATE Users SET Password = @Password WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
                    command.Parameters.AddWithValue("@Password", newPassword);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}