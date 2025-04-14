using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
