using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;

namespace CFM.Controllers
{
    [Authentication]
    public class MainController : Controller
    {
        private Coffee_ManagementContext dbContext = null;
        private readonly IProductRepository productRepository;
        private readonly ITableRepository tableRepository;
        public MainController()
        {
            dbContext = new Coffee_ManagementContext();
            productRepository = new ProductRepository();
            tableRepository = new TableRepository();
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