using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.Repository;

namespace CFM.Controllers
{
    public class MainController : Controller
    {

        private readonly IProductRepository productRepository;
        private readonly ITableRepository tableRepository;
        public MainController()
        {
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
    }
}