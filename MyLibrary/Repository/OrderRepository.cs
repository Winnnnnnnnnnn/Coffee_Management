using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;


namespace MyLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetOrders() => OrderDAO.Instance.GetOrderList();
        public Order GetOrderByID(int OrderId) => OrderDAO.Instance.GetOrderByID(OrderId);
        public void InsertOrder(Order Order) => OrderDAO.Instance.AddNew(Order);
        public void DeleteOrder(int OrderId) => OrderDAO.Instance.Remove(OrderId);
        public void DeleteOrders(List<int> idsToDelete) => OrderDAO.Instance.RemoveMultiple(idsToDelete);
        public void UpdateOrder(Order Order) => OrderDAO.Instance.Update(Order);
        public IEnumerable<Order> GetOrderRange(DateTime startDate, DateTime endDate)
        {
            return OrderDAO.Instance.GetOrderList(startDate, endDate);
        }
        public decimal GetDailyRevenues(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    // Tính tổng giá trị đơn đặt hàng trong khoảng thời gian từ startDate đến endDate
                    decimal? totalRevenue = context.Orders
                        .Where(order => order.CreatedAt >= startDate && order.CreatedAt <= endDate)
                        .Sum(order => order.TotalPrice);

                    return totalRevenue ?? 0;  // Provide a default value if totalRevenue is null
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving daily revenues: " + ex.Message);
            }
        }
    }
}