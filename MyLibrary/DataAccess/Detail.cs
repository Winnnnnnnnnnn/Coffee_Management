using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Detail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
