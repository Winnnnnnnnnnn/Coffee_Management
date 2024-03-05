using System;
using System.Collections.Generic;
using System.Linq;
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
    // [Authentication]
    public class OrderController : Controller
    {
        IOrderRepository orderRepository = null;
        // public OrderController() => orderRepository = new OrderRepository();

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
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + o?.Id + "'>",
                id = o?.Id,
                table_id = "<a class='btn btn-link text-decoration-none' href='/Order/Edit/" + o?.Id + "'>" + o?.getTableName() + " </ a > ",
                user_id = o.getUserName(),
                total_price = o?.TotalPrice,
                note = o?.Note,
                created_at = o?.CreatedAt.Value.ToString("HH:mm:ss dd/MM/yyyy"),
                status = "<span class=" + (o.Status == 0 ? "text-danger" : "text-success") + ">" + o?.getStatus() + "</span>",
                action = "<form action='/Order/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + o?.Id + "' data-id='" + o?.Id + "'/> <button type='submit' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
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
        public ActionResult Create(Order Order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Order.UserId = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user")).Id;
                    orderRepository.InsertOrder(Order);
                    var dbContext = new Coffee_ManagementContext();
                    var table = dbContext.Tables.FirstOrDefault(t => t.Id == Order.TableId);
                    if (table != null)
                    {
                        table.Status = 1;
                        dbContext.SaveChanges();
                    }
                }
                else
                {

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(Order);
            }
        }


        public object Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Order = orderRepository.GetOrderByID(id.Value);
            if (Order == null)
            {
                return NotFound();
            }
            return Json(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order Order)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid)
                {
                    return View("_ModalEditOrder", Order);
                }
                // Tiến hành cập nhật thông tin người dùng
                orderRepository.UpdateOrder(Order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi phù hợp
                ViewBag.Message = ex.Message;
                return View(Order);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                orderRepository.DeleteOrder(id);
                // Phản hồi về thành công khi xóa thành công
                return Ok(new { message = "Xóa đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi xóa đơn hàng: " + ex.Message });
            }
        }
    }
}