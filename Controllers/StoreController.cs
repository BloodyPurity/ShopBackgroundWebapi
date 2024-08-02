using Microsoft.AspNetCore.Mvc;

namespace ShopBackgroundSystem.Controllers
{
    public class StoreController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
