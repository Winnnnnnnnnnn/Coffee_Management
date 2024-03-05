using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Object { get; set; }
        public int ObjectId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User User { get; set; }

        public string getUserName()
        {
            UserDAO table = new UserDAO();
            var name = table.GetUserByID(this.UserId).Name;
            return name;
        }
    }
}
