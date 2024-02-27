using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class User
    {
        public User()
        {
            Logs = new HashSet<Log>();
            Orders = new HashSet<Order>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được trống")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Họ tên phải từ 5 đến 50 ký tự")]
        public string Name { get; set; }
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Email không được trống")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_+&*-]+(?:\\.[a-zA-Z0-9_+&*-]+)*@" + "(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Email phải đúng định dạng")]
        public string Email { get; set; }

        // [Required(ErrorMessage = "Mật khẩu không được trống")]
        // [StringLength(20, MinimumLength = 5, ErrorMessage = "Mật khẩu phải từ 5 đến 20 ký tự")]
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Điện thoại không được trống")]
        public string Phone { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
