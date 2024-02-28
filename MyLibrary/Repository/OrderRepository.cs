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
    }
}