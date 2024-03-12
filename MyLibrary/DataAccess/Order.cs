using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Order
    {
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

        // public List<Product> Products { get; set; }
        // public List<Table> Tables { get; set; }

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
            UserDAO dao = new UserDAO();
            var name = dao.GetUserByID(this.UserId).Name;
            return name;
        }

        public Table GetTable()
        {
            if (this.TableId != null)
            {
                TableDAO dao = new TableDAO();
                var table = dao.GetTableByID(this.TableId);
                return table;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Detail> GetDetail()
        {
            var context = new Coffee_ManagementContext();
            var details = context.Details.Where(d => d.OrderId == this.Id).ToList();
            return details;
        }

        public User GetUser()
        {
            UserDAO dao = new UserDAO();
            var user = dao.GetUserByID(this.UserId);
            return user;
        }

        public void RemoveDetails()
        {
            var context = new Coffee_ManagementContext();
            var details = context.Details.Where(d => d.OrderId == this.Id).Select(d => d.Id).ToList();
            DetailDAO dao = new DetailDAO();
            dao.RemoveMultiple(details);
        }

        public virtual Table Table { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
        public virtual ICollection<Factor> Factors { get; set; }
    }
}
