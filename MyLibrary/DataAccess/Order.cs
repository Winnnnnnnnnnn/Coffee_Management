using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Order
    {
        public virtual ICollection<Detail> Details { get; set; }
        public virtual ICollection<Factor> Factors { get; set; }
        public Order()
        {
            Details = new HashSet<Detail>();
            Factors = new HashSet<Factor>();
        }

        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? TableId { get; set; }
        [Required]
        public int Status { get; set; }
        public string Note { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatetedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public string getStatus()
        {
            return (this.Status == 0) ? "Chưa thanh toán" : "Đã thanh toán";
        }

        public string getTableName()
        {
            if (this.TableId != null)
            {
                TableDAO table = new TableDAO();
                var name = table.GetTableByID(this.TableId).Name;
                return name;
            }
            return "Mang đi";
        }

        public string getUserName()
        {
            UserDAO table = new UserDAO();
            var name = table.GetUserByID(this.UserId).Name;
            return name;
        }

        public virtual Table Table { get; set; }
        public virtual User User { get; set; }
    }
}
