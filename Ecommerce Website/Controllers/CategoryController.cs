using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
   
    public class CategoryController : Controller
    {
        private readonly IRepository<CategoryModel> _categoryRepo;
        public CategoryController(IRepository<CategoryModel> categoryRepo)
        {
            _categoryRepo = categoryRepo ;

        }
        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);

        }
        [HttpGet]
        public IActionResult CategoryDetails(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoriesViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View("CategoryDetails", viewModel);
        }
    }
}
