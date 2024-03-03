﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class User
    {
        public User()
        {
            Logs = new HashSet<Log>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được bỏ trống!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên phải từ 5 đến 50 ký tự!")]
        public string Name { get; set; }
        public int Role { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_+&*-]+(?:\\.[a-zA-Z0-9_+&*-]+)*@" + "(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Email phải đúng định dạng!")]
        public string Email { get; set; }

        // [Required(ErrorMessage = "Mật khẩu không được trống")]
        // [StringLength(20, MinimumLength = 5, ErrorMessage = "Mật khẩu phải từ 5 đến 20 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống!")]
        public string Phone { get; set; }

        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
