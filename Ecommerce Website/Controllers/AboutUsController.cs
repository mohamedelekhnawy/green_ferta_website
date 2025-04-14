using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
