using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Product
    {
        public Product()
        {
            Details = new HashSet<Detail>();
        }


        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public int Id { get; set; }
        
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public string Name { get; set; }
        
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public string Unit { get; set; }
        
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public decimal Price { get; set; }
        
        [Required (ErrorMessage="Vui lòng nhập đầy đủ thông tin.")]
        public int CatalogueId { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}
