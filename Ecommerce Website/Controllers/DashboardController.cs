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
        public IActionResult Add()
        {
            return View("Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View( model);
            }
            var categories = new CategoryModel {
                Name = model.Name ,
                Description=model.Description,
                CreatedOn = DateTime.Now,
            };
            
            _context.Categories.Add(categories);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            var viewModel = new CategoriesViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description 
            };

            return View("EditForm", viewModel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditForm", model); 
            }

            var category = _context.Categories.Find(model.Id);
            if (category is null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.Description = model.Description; 
            category.UpdatedOn = DateTime.Now; 

            _context.Categories.Update(category); 
            _context.SaveChanges(); 

            return RedirectToAction("Categories"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category); 
            _context.SaveChanges();

            return RedirectToAction("Categories"); 
        }

    }
}
