using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.DataAccess;
using MyLibrary.Repository;

namespace CFM.Controllers
{
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
        public IActionResult BankUpdate(string bank_id, string bank_number, string bank_template, string bank_content)
        {
            System.Console.WriteLine(bank_template);
            UpdateSetting("bank_id", bank_id);
            UpdateSetting("bank_number", bank_number);
            UpdateSetting("bank_template", bank_template);
            UpdateSetting("bank_content", bank_content);
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