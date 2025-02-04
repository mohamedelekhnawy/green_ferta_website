using Ecommerce_Website.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecommerce_Website.Controllers
{

    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Form(CategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var categories = new CategoryModel {Name = model.Name ,Description=model.Description};
            
            _context.Categories.Add(categories);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }
    }
}
