using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLibrary.DataAccess;

namespace CFM.Controllers
{
    public class LogController : Controller
    {
        LogDAO dao = new LogDAO();

        public ActionResult Index()
        {
            ViewBag.IsActive = "order";
            return View();
        }
    public IActionResult Load()
        {
            var logs = dao.GetLogList();
            var data = logs.Select(o => new
            {
                checkbox = "<input type='checkbox' class='form-check-input choice' name='choices[]' value='" + o?.Id + "'>",
                id = o?.Id,
                user_id = o.getUserName(),
                action = o.Action,
                object_name = o.Object,
                object_id = o.ObjectId,
                created_at = o?.CreatedAt.Value.ToString("HH:mm:ss dd/MM/yyyy"),
            });
            return Json(new { data = data });
        }
    }
    


}