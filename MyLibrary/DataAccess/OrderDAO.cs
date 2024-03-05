using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLibrary.DataAccess;


namespace MyLibrary.DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Order> GetOrderList()
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Orders.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Order list: " + ex.Message);
            }
        }
        public IEnumerable<Order> GetOrderList(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    var orders = context.Orders
                        .Where(order => order.CreatedAt >= startDate && order.CreatedAt <= endDate)
                        .ToList();
                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Order list: " + ex.Message);
            }
        }



        public Order GetOrderByID(int Id)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Orders.FirstOrDefault(m => m.Id == Id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Order by ID: " + ex.Message);
            }
        }


        public void AddNew(Order Order)
        {
            try
            {
                var existingOrder = GetOrderByID(Order.Id);
                if (existingOrder == null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Orders.Add(Order);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Order already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new Order: " + ex.Message);
            }
        }

        public void Update(Order Order)
        {
            try
            {
                var existingOrder = GetOrderByID(Order.Id);
                if (existingOrder != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Orders.Update(Order);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Order does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Order: " + ex.Message);
            }
        }


        public void Remove(int Id)
        {
            try
            {
                var OrderToRemove = GetOrderByID(Id);
                if (OrderToRemove != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Orders.Remove(OrderToRemove);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Order does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing Order: " + ex.Message);
            }
        }

        public void RemoveMultiple(List<int> Ids)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    foreach (var Id in Ids)
                    {
                        var OrderToRemove = context.Orders.FirstOrDefault(u => u.Id == Id);
                        if (OrderToRemove != null)
                        {
                            context.Orders.Remove(OrderToRemove);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing Orders: " + ex.Message);
            }
        }

        public decimal getNumberProduct(List<int> ints)
        {
            int numberProduct = 0;
            var context = new Coffee_ManagementContext();
            var details = context.Details.ToList();
            foreach (var item in details)
            {
                if (ints.Contains(item.OrderId))
                {
                    numberProduct += item.Quantity;
                }
            }
            return numberProduct;
        }
    }
}