using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
    }
}
