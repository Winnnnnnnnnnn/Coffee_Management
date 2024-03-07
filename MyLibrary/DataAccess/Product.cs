using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Product
    {
        public Product()
        {
            Details = new HashSet<Detail>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin.")]
        public string Name { get; set; }
        public string Image { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin.")]
        public string Unit { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin.")]
        public int Catalogue { get; set; }
        
        public virtual ICollection<Detail> Details { get; set; }
    }
}
