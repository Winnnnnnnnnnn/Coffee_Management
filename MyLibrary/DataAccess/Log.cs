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
        public int ObjectId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}
