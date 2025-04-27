using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IProductRepository _productRepository;
        public ProductController(IRepository<Product> productRepo,IProductRepository ProductRepository)
        {
            _productRepo = productRepo;
            _productRepository = ProductRepository;
        }
        public IActionResult Index()
        {
            var Product = _productRepo.GetAll();
            return View(Product);

        }
        public IActionResult ProductDetails(int Id)
        {
            var product = _productRepo.GetById(Id);
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Importance = product.Importance,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                ImageUrl = product.ImageUrl,
                HowToUse = product.HowToUse,
                IsAvailable = product.IsAvailable,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
            return View("ProductDetails", viewModel);
        }
        [HttpGet]
        public IActionResult ProductsByCategory(int categoryId)
        {
            // Fetch products for the given category ID
            var products = _productRepository.GetProductsByCategoryId(categoryId);

            if (!products.Any())
            {
                return View("NoProductsFound");
            }

            // Pass the products to the view
            return View(products);
        }
        public ActionResult Search(string query)
        {
            var results = _productRepository.Search(query);
            return View("Index", results);
        }
        [HttpGet]
        public JsonResult AutoComplete(string term)
        {
            var results = _productRepository.Search(term)
                .Select(p => new
                {
                    label = p.Name,
                    id = p.Id
                })
                .Take(10)
                .ToList();

            return Json(results);
        }

    }
}
