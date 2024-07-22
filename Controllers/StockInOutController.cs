using Microsoft.AspNetCore.Mvc;

namespace ShopBackgroundSystem.Controllers
{
    public class StockInOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
