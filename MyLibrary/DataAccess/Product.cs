using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Image { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Catalogue { get; set; }

        public virtual ICollection<Detail> Details { get; set; }
    }
}
