using Ecommerce_Website.Core.ViewModels;
using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_Website.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository<CategoryModel> _categoryRepo;
        private readonly IRepository<Product> _productRepo;

        public DashboardController(IRepository<CategoryModel> categoryRepo, IRepository<Product> productRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = new CategoryModel
            {
                Name = model.Name,
                Description = model.Description,
                CreatedOn = DateTime.Now
            };

            _categoryRepo.Add(category);
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult Edit(int id)
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

            var category = _categoryRepo.GetById(model.Id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.UpdatedOn = DateTime.Now;

            _categoryRepo.Update(category);
            return RedirectToAction("Categories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _categoryRepo.Delete(id);
            return RedirectToAction("Categories");
        }

        // Products
        [HttpGet]
        public IActionResult Product()
        {
            var products = _productRepo.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new ProductsViewModel
            {
                Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };

            return View(model);
        }

        // ✅ إضافة منتج جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Importance = model.Importance,
                Price = model.Price,
                DiscountedPrice = model.DiscountedPrice,
                CategoryId = model.SelectedCategoryId,
                Quantity = model.Quantity,
                IsAvilable = model.IsAvilable,
                ImageUrl = model.ImageUrl,
                ImageUrls = model.ImageUrls ?? new List<string>(),
                CreatedAt = DateTime.Now
            };

            _productRepo.Add(product);
            return RedirectToAction("Product");
        }
    }
}
