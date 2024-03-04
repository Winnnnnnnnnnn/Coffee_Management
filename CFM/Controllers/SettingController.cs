using Microsoft.AspNetCore.Mvc;
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

    }
}