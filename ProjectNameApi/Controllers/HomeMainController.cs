using Microsoft.AspNetCore.Mvc;

namespace ProjectNameApi.Controllers
{
    public class HomeMainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexHome()
        {
            return RedirectToAction("Index");
        }
    }
}
