using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.DataAccess;
using MyLibrary.Repository;
using MyMVC.Models.Authentication;

namespace CFM.Controllers
{
    [Authentication]
    public class SettingController : Controller
    {
        ISettingRepository settingRepository = null;
        public SettingController() => settingRepository = new SettingRepository();
        public ActionResult Index()
        {
            ViewBag.IsActive = "setting";
            return View();
        }

        public IActionResult Load()
        {
            var settingList = settingRepository.GetSettings();
            return Json(settingList);
        }

        [HttpPost]
        public IActionResult BankUpdate(IFormCollection request)
        {
            foreach (var key in request.Keys)
            {
                UpdateSetting(key, request[key]);
            }
            return RedirectToAction("Index");
        }


        private void UpdateSetting(string key, string value)
        {
            Coffee_ManagementContext context = new Coffee_ManagementContext();
            var setting = context.Settings.SingleOrDefault(s => s.Key == key);
            if (setting != null)
            {
                setting.Value = value;
                context.SaveChanges();
            }
        }
    }
}