using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;
using Newtonsoft.Json;

namespace CFM.Controllers
{
    [Authentication]
    public class MainController : Controller
    {
        private Coffee_ManagementContext dbContext = null;
        private readonly IProductRepository productRepository;
        private readonly IDetailRepository detailRepository;
        private readonly ITableRepository tableRepository;
        IOrderRepository orderRepository = null;
        public MainController()
        {
            dbContext = new Coffee_ManagementContext();
            productRepository = new ProductRepository();
            tableRepository = new TableRepository();
            detailRepository = new DetailRepository();
            orderRepository = new OrderRepository();
        }
        public IActionResult Index()
        {
            return View();
        }

        public object Load()
        {
            return Json(new
            {
                products = productRepository.GetProducts(),
                tables = tableRepository.GetTables(),
            });
        }

        public object Create(IFormCollection request)
        {
            Order order = new Order();
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
            order.UserId = user.Id;
            order.Status = 0;
            order.TableId = int.Parse(request["table_id"]);
            order.TotalPrice = int.Parse(request["total_price"]);
            orderRepository.InsertOrder(order);

            //Tạo detail
            for (int i = 0; i < request["quantity[]"].Count(); i++)
            {
                Detail detail = new Detail();
                detail.OrderId = order.Id;
                detail.ProductId = int.Parse(request["id[]"][i]);
                detail.Quantity = int.Parse(request["quantity[]"][i]);
                detail.Price = int.Parse(request["price[]"][i]);
                detailRepository.InsertDetail(detail);
            }
            var table = dbContext.Tables.FirstOrDefault(t => t.Id == order.TableId);
            if (table != null)
            {
                table.Status = 1;
                dbContext.SaveChanges();
            }
            return Json(new
            {
                msg = "Đã tạo đơn hàng thành công",
                status = "success",
            });
        }

        [HttpPost]
        public object Edit(IFormCollection request)
        {
            System.Console.WriteLine(int.Parse(request["id"]));
            Order order = orderRepository.GetOrderByID(int.Parse(request["id"]));
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
            order.UserId = user.Id;
            order.TotalPrice = int.Parse(request["total_price"]);
            orderRepository.UpdateOrder(order);
            order.RemoveDetails();
            //Tạo detail
            for (int i = 0; i < request["quantity[]"].Count(); i++)
            {
                Detail detail = new Detail();
                detail.OrderId = order.Id;
                detail.ProductId = int.Parse(request["id[]"][i]);
                detail.Quantity = int.Parse(request["quantity[]"][i]);
                detail.Price = int.Parse(request["price[]"][i]);
                detailRepository.InsertDetail(detail);
            }

            return Json(new
            {
                msg = "Đã cập nhật đơn hàng thành công",
                status = "success",
            });
        }

        public object GetOrderByTableId(int id)
        {
            Order order = dbContext.Orders.FirstOrDefault(o => o.TableId == id && o.Status == 0);
            if (order != null)
            {
                return Json(new
                {
                    order = order,
                    details = order.GetDetail(),
                    table = order.GetTable(),
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}