using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;


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
            var OrderList = orderRepository.GetOrders();
            return View(OrderList);
        }


        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order Order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    orderRepository.InsertOrder(Order);
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