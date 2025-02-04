using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Form()
        {
            return View();
        }

    }
}
