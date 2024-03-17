using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;
using Newtonsoft.Json;


namespace CFM.Controllers
{
    [Authentication]
    public class OrderController : Controller
    {
        IOrderRepository orderRepository = null;
        public OrderController() => orderRepository = new OrderRepository();

        public ActionResult Index()
        {
            ViewBag.IsActive = "order";
            return View();
        }

        public IActionResult Load()
        {
            var orders = orderRepository.GetOrders();
            var data = orders.Select(o => new
            {
                id = o?.Id,
                table_id = "<a class='btn btn-link text-decoration-none' href='/Order/Edit/" + o?.Id + "'>" + o?.getTableName() + " </ a > ",
                user_id = o.getUserName(),
                total_price = o?.TotalPrice,
                note = o?.Note,
                created_at = o?.CreatedAt.Value.ToString("HH:mm:ss dd/MM/yyyy"),
                status = "<span class=" + (o.Status == 0 ? "text-danger" : "text-success") + ">" + o?.getStatus() + "</span>",
                action = "<div class='d-flex'><a class='btn text-dark btn-print-bill me-2' data-id='" + o?.Id + "'><i class='bi bi-printer'></i></a><a class='btn text-success btn-pay me-2' data-id='" + o?.Id + "'><i class='bi bi-currency-dollar'></i></a><form action='/Order/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + o?.Id + "' data-id='" + o?.Id + "'/> <button type='submit' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form></div>"
            });
            return Json(new { data = data });
        }

        public ActionResult Create()
        {
            ViewBag.IsActive = "order";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order Order, List<Detail> details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    Order.UserId = user.Id;
                    orderRepository.InsertOrder(Order);
                    var dbContext = new Coffee_ManagementContext();
                    var table = dbContext.Tables.FirstOrDefault(t => t.Id == Order.TableId);
                    if (table != null)
                    {
                        table.Status = 1;
                        dbContext.SaveChanges();
                    }
                    IDetailRepository detailRepository = new DetailRepository();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = user.Id,
                        Action = "Đã tạo",
                        Object = "Đơn hàng",
                        ObjectId = Order.Id,
                    });
                    dbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(Order);
            }
        }

        public ActionResult getOrder(int id)
        {
            var order = orderRepository.GetOrderByID(id);
            if (order == null)
            {
                return NotFound();
            }
            var context = new Coffee_ManagementContext();
            ViewBag.IsActive = "order";
            return Json(new
            {
                order = order,
                table = order.GetTable(),
                user = order.GetUser(),
                details = order.GetDetail(),
                products = context.Products.ToList(),
            });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = orderRepository.GetOrderByID(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.IsActive = "order";
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            System.Console.WriteLine("Error: " + error.ErrorMessage);
                        }
                    }
                    return View("Index", order);
                }
                Order ord = orderRepository.GetOrderByID(id);
                order.CreatedAt = ord.CreatedAt;
                order.RemoveDetails();
                var dbContext = new Coffee_ManagementContext();
                if (ord.TableId != order.TableId)
                {
                    dbContext.Tables.FirstOrDefault(t => t.Id == ord.TableId).Status = 0;
                    dbContext.Tables.FirstOrDefault(t => t.Id == order.TableId).Status = 1;
                }
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                order.UserId = user.Id;
                orderRepository.UpdateOrder(order);
                LogDAO dao = new LogDAO();
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    Action = "Đã cập nhật",
                    Object = "Sản phẩm",
                    ObjectId = order.Id,
                });
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            object response = null;
            try
            {
                if (orderRepository.GetOrderByID(id) == null)
                {
                    response = new
                    {
                        controller = "Order",
                        title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                        status = "danger"
                    };
                }
                else
                {
                    orderRepository.DeleteOrder(id);
                    response = new
                    {
                        controller = "Order",
                        title = "Đã xóa thành công.",
                        status = "success"
                    };
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    var dbContext = new Coffee_ManagementContext();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = user.Id,
                        Action = "Đã xóa",
                        Object = "Đơn hàng",
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
                    controller = "Table",
                    title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                    status = "danger"
                };
                return Json(response);
            }
        }

        [HttpPost]
        public ActionResult Pay(int id, int payment)
        {
            try
            {
                Order order = orderRepository.GetOrderByID(id);
                order.Status = payment;
                orderRepository.UpdateOrder(order);
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                order.UserId = user.Id;
                orderRepository.UpdateOrder(order);
                var dbContext = new Coffee_ManagementContext();
                if (order.TableId != null)
                {
                    var table = dbContext.Tables.FirstOrDefault(t => t.Id == order.TableId);
                    table.Status = 0;
                    dbContext.SaveChanges();
                }
                LogDAO dao = new LogDAO();
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    Action = "Đã thanh toán",
                    Object = "Đơn hàng",
                    ObjectId = order.Id,
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server");
            }
        }

        public ActionResult Bill(int order_id)
        {
            ViewBag.IsActive = order_id + "";
            return View("Bill");
        }
    }
}