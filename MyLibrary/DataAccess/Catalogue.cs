using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Catalogue
    {
        public Catalogue()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
