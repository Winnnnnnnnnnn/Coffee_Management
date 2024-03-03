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
    // [Authentication]
    public class ProductController : Controller
    {
        IProductRepository productRepository = null;
        public ProductController() => productRepository = new ProductRepository();

        public ActionResult Index()
        {
            ViewBag.IsActive = "product";
            return View();
        }

        public IActionResult Load()
        {
            var products = productRepository.GetProducts();
            var data = products.Select(p => new
            {
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + p.Id + "'>",
                id = p.Id,
                name = "<a class='btn btn-link text-decoration-none' href='/Product/Edit/" + p.Id + "'>" + p.Name + " </ a > ",
                unit = p.Unit,
                price = p.Price,
                catalogue_id = "" + p.Catalogue,
                action = "<form action='/Product/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + p.Id + "' data-id='" + p.Id + "'/> <button type='submit' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });
            return Json(new { data = data });
        }

        public IActionResult Create()
        {
            ViewBag.IsActive = "product";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(product);
                }
                else
                {
                    productRepository.InsertProduct(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = productRepository.GetProductByID(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid)
                {

                    return View("Index", product);
                }
                // Tiến hành cập nhật thông tin vai trò
                productRepository.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            System.Console.WriteLine(id);
            if (id == null)
            {
                return NotFound();
            }
            productRepository.DeleteProduct(int.Parse(id));
            return RedirectToAction("Index");
        }
    }
}