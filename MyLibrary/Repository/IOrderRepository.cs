using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;


namespace MyLibrary.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrderByID(int OrderId);
        void InsertOrder(Order Order);
        void DeleteOrder(int OrderId);
        void DeleteOrders(List<int> OrderIds);
        void UpdateOrder(Order Order);
    }
}