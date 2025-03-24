using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        public ProductController(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
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

    }
}
