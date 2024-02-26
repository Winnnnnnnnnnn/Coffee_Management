﻿using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
