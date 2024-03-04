using System;
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
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public string Name { get; set; }
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public int Role { get; set; }
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public string Email { get; set; }
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public string Password { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
