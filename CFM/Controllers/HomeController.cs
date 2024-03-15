using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CFM.Models;
using Microsoft.AspNetCore.Authorization;
using MyMVC.Models.Authentication;
using MyLibrary.Repository;
using System.Globalization;
using MyLibrary.DataAccess;

namespace CFM.Controllers
{
    // [Authentication]
    public class HomeController : Controller
    {
        IOrderRepository orderRepository = null;
        public HomeController() => orderRepository = new OrderRepository();

        public IActionResult Index()
        {
            ViewBag.IsActive = "dashboard";
            return View();
        }
        public IActionResult Load(string range)
        {
            try
            {
                List<int> list = new List<int> { };
                var dateRange = range.Split(" - ");
                var start = DateTime.ParseExact(dateRange[0], "d/M/yyyy", CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact(dateRange[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                var DailiRevenues = orderRepository.GetOrderRange(start, end);
                foreach (var item in DailiRevenues)
                {
                    list.Add(item.Id);
                }
                return Json(new
                {
                    numberProducts = new OrderDAO().getNumberProduct(list),
                    DailiRevenues = DailiRevenues,
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }
    }
}
