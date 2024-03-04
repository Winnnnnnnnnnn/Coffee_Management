using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Khu vực không được trống")]
        public string Area { get; set; }
        public string Note { get; set; }

        public int? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
