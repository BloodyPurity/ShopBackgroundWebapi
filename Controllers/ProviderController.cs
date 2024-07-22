using Microsoft.AspNetCore.Mvc;

namespace ShopBackgroundSystem.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
