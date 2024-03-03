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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Table table)
        {
            if (table.Name == null)
            {
                ViewData["TableName"] = "Tên bàn không được để trống!";
            }
            if (table.Area == null)
            {
                ViewData["TableArea"] = "Khu vực không được để trống!";
            }
            try
            {
                if (ModelState.IsValid)
                {
                    tableRepository.InsertTable(table);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(table);
            }
        }

        public object Edit(int? id)
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
            return Json(table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Table table)
        {
            try
            {
                if (table.Name == null)
                {
                    ViewData["TableName"] = "Tên bàn không được để trống!";
                }
                if (table.Area == null)
                {
                    ViewData["TableArea"] = "Khu vực không được để trống!";
                }
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            System.Console.WriteLine(id);
            tableRepository.DeleteTable(id);
            return RedirectToAction("Index");
        }
    }
}
