using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductController : Controller
    {
        // IProductRepository productRepository = null;


        // public ProductController() => productRepository = new ProductRepository();

        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
            productRepository = new ProductRepository();
        }


        public ActionResult Index()
        {
            ViewBag.IsActive = "product";
            return View();
        }

        public IActionResult Load()
        {
            var products = productRepository.GetProducts();
            int recordsTotal = products.Count();
            var data = products.Select(p => new
            {
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + p.Id + "'>",
                id = p.Id,
                name = "<a class='btn btn-link text-decoration-none' href='/Product/Edit/" + p.Id + "'>" + p.Name + " </ a >",
                unit = p.Unit,
                price = p.Price,
                catalogue_id = p.getCatalogueName(),
                image = p.Image,
                action = "<form action='/Product/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + p.Id + "' data-id='" + p.Id + "'/> <button type='button' class='btn btn-link text-decoration-none btn-remove-product'><i class='bi bi-trash3'></i></button></form>"
            });

            var language = new
            {
                sProcessing = "Đang xử lý...",
                sLengthMenu = "_MENU_ dòng /  trang",
                emptyTable = "Không có dữ liệu",
                sZeroRecords = "Không có kết quả nào được tìm thấy",
                sInfo = "Hiển thị từ _START_ đến _END_ của _TOTAL_ mục",
                sInfoEmpty = "Hiển thị từ 0 đến 0 của 0 mục",
                sInfoFiltered = "(đã lọc từ _MAX_ mục)",
                sInfoPostFix = "",
                sSearch = "Tìm kiếm:",
                sUrl = "",
                oPaginate = new
                {
                    sFirst = "&laquo;",
                    sLast = "&raquo;",
                    sNext = "&rsaquo;",
                    sPrevious = "&lsaquo;"
                }
            };

            return Json(new
            {
                products = products,
                recordsTotal = recordsTotal,
                data = data,
                language = language
            });
        }

        public IActionResult Create()
        {
            ViewBag.IsActive = "product";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, IFormFile imageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Lưu ảnh vào thư mục wwwroot/images
                        var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                        // Lưu đường dẫn của ảnh vào trường Image của đối tượng Product
                        product.Image = "~/img/" + uniqueFileName;
                    }
                    else
                    {
                        product.Image = "~/img/placeholder.jpg";
                    }
                    productRepository.InsertProduct(product);
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    var dbContext = new Coffee_ManagementContext();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = user.Id,
                        Action = "Đã tạo",
                        Object = "Sản phẩm",
                        ObjectId = product.Id,
                    });
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Index");
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
            ViewBag.IsActive = "product";
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product, IFormFile newImageFile)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid)
                {
                    return View("Index", product);
                }
                if (newImageFile != null && newImageFile.Length > 0)
                {
                    // Lưu ảnh vào thư mục wwwroot/images
                    var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + newImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        newImageFile.CopyTo(fileStream);
                    }

                    // Cập nhật đường dẫn hình ảnh mới cho sản phẩm
                    product.Image = "~/img/" + uniqueFileName;
                }
                else
                {
                    // Nếu không có hình ảnh mới được tải lên, giữ nguyên đường dẫn hình ảnh cũ
                    var existingProduct = productRepository.GetProductByID(id);
                    if (existingProduct != null)
                    {
                        product.Image = existingProduct.Image;
                    }
                }
                productRepository.UpdateProduct(product);
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                var dbContext = new Coffee_ManagementContext();
                LogDAO dao = new LogDAO();
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    Action = "Đã cập nhật",
                    Object = "Sản phẩm",
                    ObjectId = product.Id,
                });
                dbContext.SaveChanges();
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
        public ActionResult Delete(int id)
        {
            object response = null;
            try
            {
                if (productRepository.GetProductByID(id) == null)
                {
                    response = new
                    {
                        title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                        status = "danger"
                    };
                    return Json(response);
                }
                else
                {
                    ProductDAO DAO = new ProductDAO();
                    DAO.DeleteDetailsByProductId(id);
                    productRepository.DeleteProduct(id);
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    response = new
                    {
                        controller = "Product",
                        title = "Đã xóa thành công.",
                        status = "success"
                    };
                    var dbContext = new Coffee_ManagementContext();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = user.Id,
                        Action = "Đã xóa",
                        Object = "Sản phẩm",
                        ObjectId = id,
                    });
                    dbContext.SaveChanges();
                }
                return Json(response);
            }
            catch (System.Exception)
            {
                response = new
                {
                    title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                    status = "danger"
                };
                return Json(response);
            }

        }
    }
}