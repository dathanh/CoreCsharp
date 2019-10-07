using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Controllers.Base;
using ProjectName.Models.Base;

namespace ProjectName.Controllers
{
    public class MainContainerController : ApplicationControllerBase
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetMenu()
        {
            var modelBase = new ViewModelBase();
            return Json(new { Error = string.Empty, Data = new { Menu = modelBase.MenuViewModel } });
        }
    }
}