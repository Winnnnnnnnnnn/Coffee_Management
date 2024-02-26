using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Order
    {
        public Order()
        {
            Details = new HashSet<Detail>();
            Factors = new HashSet<Factor>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TableId { get; set; }
        public DateTime? CheckoutAt { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Table Table { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
        public virtual ICollection<Factor> Factors { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
