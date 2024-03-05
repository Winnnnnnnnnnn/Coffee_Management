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
    public class TableController : Controller
    {
        ITableRepository tableRepository = null;
        public TableController() => tableRepository = new TableRepository();

        public ActionResult Index()
        {
            ViewBag.IsActive = "table";
            var tableList = tableRepository.GetTables();
            return View("~/Views/Table/Index.cshtml", tableList);
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
            }
            return RedirectToAction(nameof(Index));
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
                tableRepository.UpdateTable(table);
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
                        title = "Đã có lỗi xảy ra trong quá trình xóa! Vui lòng thử lại sau.",
                        status = "danger"
                    };
                    return Json(response);
                }
                else
                {
                    response = new
                    {
                        controller = "Table",
                        title = "Đã xóa thành công.",
                        status = "success"
                    };
                    tableRepository.DeleteTable(id);
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
