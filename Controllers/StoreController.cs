using Microsoft.AspNetCore.Mvc;

namespace ShopBackgroundSystem.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
