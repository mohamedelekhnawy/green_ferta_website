using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class ProductFiltersController : Controller
    {
        private readonly IRepository<ProductFilter> _ProductFilterRepo;
        private readonly IRepository<Product> _ProductRepo;

        public ProductFiltersController(IRepository<ProductFilter> productFilterRepo, IRepository<Product> productRepo)
        {
            _ProductFilterRepo = productFilterRepo;
            _ProductRepo = productRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductFilter()
        {
            var productFilters = _ProductFilterRepo.GetAll();
            return View(productFilters);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductFilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                return View(filter);
            }
            var productFilter = new ProductFilter
            {
                Size=filter.ProductSize,
            };

            _ProductFilterRepo.Add(productFilter);
            return RedirectToAction("ProductFilter");
        }
        public IActionResult Edit(int id) 
        {
            var productFilters = _ProductFilterRepo.GetById(id);
            if (productFilters == null)
            {
                return NotFound();
            }
            return View(productFilters);
        }
        [HttpPost]
        public IActionResult Edit(ProductFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return View(filter);
            }
            var productFilter = _ProductFilterRepo.GetById(filter.Id);
            if (productFilter == null)
            {
                return NotFound();
            }
            productFilter.Size = filter.Size;
            _ProductFilterRepo.Update(productFilter);
            return RedirectToAction("ProductFilter");
        }
        [HttpPost]
        public IActionResult Delete(int id) 
        {
            var productFilter = _ProductFilterRepo.GetById(id);
            if (productFilter == null)
            {
                return NotFound();
            }
            _ProductFilterRepo.Delete(id);
            return RedirectToAction("ProductFilter");
        }
    }
}
