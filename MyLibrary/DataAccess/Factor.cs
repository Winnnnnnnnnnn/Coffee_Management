using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Factor
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Order Order { get; set; }
    }
}
