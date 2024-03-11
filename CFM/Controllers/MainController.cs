using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;
using MyLibrary.Repository;

namespace CFM.Controllers
{
    public class MainController : Controller
    {
        ITableRepository tableRepository = null;
        IProductRepository productRepository = null;
        public MainController()
        {
            tableRepository = new TableRepository();
            productRepository = new ProductRepository();
        }

        public IActionResult Index()
        {
            var MainModel = new MainModel
            {
                TableList = (List<Table>)tableRepository.GetTables(),
                ProductList = (List<Product>)productRepository.GetProducts(),
            };
            var i = 0;
            foreach (var item in MainModel.TableList)
            {
                Console.WriteLine(item.Name + " v    ");
                Console.WriteLine(++i);
            }
            return View(MainModel);
        }

    }
}