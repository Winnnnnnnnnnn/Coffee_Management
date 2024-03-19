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
    public class TableController : Controller
    {
        ITableRepository tableRepository = null;
        public TableController() => tableRepository = new TableRepository();

        public ActionResult Index()
        {
            ViewBag.IsActive = "table";
            return View();
        }

        public IActionResult Load()
        {
            var tables = tableRepository.GetTables();
            var data = tables.Select(t => new
            {
                name = "<a class='btn btn-link text-decoration-none' href='/Table/Edit/" + t?.Id + "'>" + t?.Name + " </ a > ",
                area = t?.Area,
                note = t?.Note,
                status = t?.GetStatus(),
                action = "<form action='/Table/Delete' method='POST' class='save-form'><input type='hidden' name='id' value='" + t?.Id + "' data-id='" + t?.Id + "'/> <button type='submit' class='btn btn-link text-decoration-none btn-remove'><i class='bi bi-trash3'></i></button></form>"
            });
            return Json(new { data = data });
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Table table)
        {
            if (ModelState.IsValid)
            {
                tableRepository.InsertTable(table);
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                var dbContext = new Coffee_ManagementContext();
                LogDAO dao = new LogDAO();
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    Action = "Đã tạo",
                    Object = "Bàn",
                    ObjectId = table.Id,
                });
                dbContext.SaveChanges();
            }
            else
            {
                return View();
            }
            ViewBag.IsActive = "table";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult GetTableFree(Table table)
        {
            var context = new Coffee_ManagementContext();
            var tables = context.Tables.Where(t => t.Status == 0).ToList();
            return Json(tables);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var table = tableRepository.GetTableByID(id.Value);
            if (table == null)
            {
                return NotFound();
            }
            return View("Edit", table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Table table)
        {
            try
            {
                System.Console.WriteLine(id + " " + Json(table));
                table.Id = id;
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                var dbContext = new Coffee_ManagementContext();
                tableRepository.UpdateTable(table);
                LogDAO dao = new LogDAO();
                System.Console.WriteLine(table.Area);
                System.Console.WriteLine(table.Id);
                dao.AddNew(new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    Action = "Đã cập nhật",
                    Object = "Bàn",
                    ObjectId = table.Id,
                });
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var tableList = tableRepository.GetTables();
                ViewBag.Message = ex.Message;
                return View("Index", tableList);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            object response = null;
            try
            {
                if (tableRepository.GetTableByID(id) == null)
                {
                    response = new
                    {
                        controller = "Table",
                        title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                        status = "danger"
                    };
                    return Json(response);
                }
                else
                {
                    tableRepository.DeleteTable(id);
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                    var dbContext = new Coffee_ManagementContext();
                    LogDAO dao = new LogDAO();
                    dao.AddNew(new Log
                    {
                        Id = 0,
                        UserId = user.Id,
                        Action = "Đã xóa",
                        Object = "Bàn",
                        ObjectId = id,
                    });
                    dbContext.SaveChanges();
                    response = new
                    {
                        controller = "Table",
                        title = "Đã xóa thành công.",
                        status = "success"
                    };
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                response = new
                {
                    controller = "Table",
                    title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                    status = "danger"
                };
                return Json(response);
            }
        }
    }
}
