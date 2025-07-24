using Ecommerce_Website.Core.ViewModels;
using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website.Controllers
{

    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly int _MaxSize = 11242880;

        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<ProductFilter> _productFilterRepo;
        private readonly IRepository<CategoryModel> _categoryRepo;
        private readonly IProductRepository _productRepository;
        public ProductController(IRepository<Product> productRepo, IProductRepository ProductRepository, IRepository<CategoryModel> categoryRepo, IWebHostEnvironment webHostEnvironment, IProductRepository productRepository, IRepository<ProductFilter> productFilterRepo)
        {
            _productRepo = productRepo;
            _productRepository = ProductRepository;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
            _productFilterRepo = productFilterRepo;
        }


        public IActionResult Index(int? categoryId, List<int> selectedSizes)
        {
            // إنشاء ViewModel رئيسي يحتوي على كل البيانات
            var mainViewModel = new ProductsViewModel
            {
                SelectedCategoryId = categoryId ?? 0,
                SelectedSizes = selectedSizes ?? new List<int>(),
                Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name ?? "غير محدد"
                    }).ToList(),
                AvailableSizes = _productFilterRepo.GetAll()
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Size ?? "غير محدد"
                    }).ToList()
            };

            // جلب المنتجات وتطبيق الفلترة
            var products = _productRepo.GetAll().AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            if (selectedSizes != null && selectedSizes.Any())
            {
                products = products.Where(p => p.ProductFilterId.HasValue &&
                    selectedSizes.Contains(p.ProductFilterId.Value));
            }

            mainViewModel.Products = products.ToList().Select(p => new ProductsViewModel
            {
                Id = p.Id,
                Name = p.Name ?? "غير محدد",
                Description = p.Description ?? "",
                Importance = p.Importance ?? "",
                Price = p.Price,
                DiscountedPrice = p.DiscountedPrice,
                ImageUrl = p.ImageUrl ?? "default.jpg",
                IsAvailable = p.IsAvailable,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                SelectedCategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : "غير محدد", // بدون ?.
                ProductFilterId = p.ProductFilterId
            }).ToList();

            return View(mainViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Product()
        {
            var products = _productRepo.GetAll() ?? new List<Product>();

            var productViewModels = products.Select(p => new ProductsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Importance = p.Importance,
                Price = p.Price,
                DiscountedPrice = p.DiscountedPrice,
                ImageUrl = p.ImageUrl,
                SelectedCategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                IsAvailable = p.IsAvailable,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                HowToUse = p.HowToUse,
            }).ToList();

            return View(productViewModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            var model = new ProductsViewModel
            {
                // Categories Dropdown
                Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList(),

                // Filters Dropdown (مثل الأحجام)
                AvailableSizes = _productFilterRepo.GetAll()
                    .Select(pf => new SelectListItem
                    {
                        Value = pf.Id.ToString(),
                        Text = pf.Size  // أو أي خاصية أخرى مثل Color
                    })
                    .ToList(),

                // تعيين القيم الافتراضية
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProductsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();

                model.AvailableSizes = _productFilterRepo.GetAll()
                    .Select(pf => new SelectListItem { Value = pf.Id.ToString(), Text = pf.Size })
                    .ToList();

                return View();
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Importance = model.Importance,
                Price = model.Price,
                DiscountedPrice = model.DiscountedPrice,
                CategoryId = model.SelectedCategoryId,
                ProductFilterId = model.ProductFilterId,
                HowToUse = model.HowToUse,
                IsAvailable = model.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            if (model.Image != null)
            {
            var extension = Path.GetExtension(model.Image.FileName).ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.Image), "Not allowed extension");
                return View(model);
            }
            if (model.Image.Length > _MaxSize)
            {
                ModelState.AddModelError(nameof(model.Image), "Maximum size is 5MB");
                return View(model);
            }
            var imageName = $"{Guid.NewGuid()}{extension}";
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Products", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                product.ImageUrl = imageName;
            }

            _productRepo.Add(product);

            return RedirectToAction("Product");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // تأكيد أن الصورة تُعرض بشكل صحيح
            var imageUrl = string.IsNullOrEmpty(product.ImageUrl) ? "/Images/Products/default.png" : product.ImageUrl;

            var model = new ProductsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Importance = product.Importance,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                SelectedCategoryId = product.CategoryId,
                HowToUse = product.HowToUse,
                IsAvailable = product.IsAvailable,
                ImageUrl = product.ImageUrl ?? "/Images/Products/default.png",
                Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList(),

                AvailableSizes = _productFilterRepo.GetAll()
                    .Select(P => new SelectListItem { Value =P.Id.ToString(),Text = P.Size})
                    .ToList()

            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductsViewModel model, IFormFile? Image)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }

            var product = _productRepo.GetById(model.Id);
            if (product == null)
            {
                return NotFound();
            }

            // ✅ تحديث بيانات المنتج
            product.Name = model.Name;
            product.Description = model.Description;
            product.Importance = model.Importance;
            product.Price = model.Price;
            product.DiscountedPrice = model.DiscountedPrice;
            product.CategoryId = model.SelectedCategoryId;
            product.HowToUse = model.HowToUse;
            product.IsAvailable = model.IsAvailable;
            product.UpdatedAt = DateTime.UtcNow;
            product.ProductFilterId = model.ProductFilterId;

            // ✅ معالجة تحديث الصورة
            if (Image != null && Image.Length > 0)
            {

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Products");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // التأكد من وجود المجلد
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // حفظ الصورة في المجلد
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                // حذف الصورة القديمة إن وجدت
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, Path.GetFileName(product.ImageUrl));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // تحديث الصورة في قاعدة البيانات
                product.ImageUrl = uniqueFileName;
            }

            _productRepo.Update(product);

            return RedirectToAction("product");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _productRepo.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            // Optional: Delete the image from wwwroot if it exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Products", product.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Delete the product
            _productRepo.Delete(id);
            return RedirectToAction("Product"); // Redirect after deletion
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult View(int Id)
        {
            var product = _productRepo.GetById(Id);
            if (product == null)
            {
                return NotFound();
            }
            var ViewModel = new ProductsViewModel
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
            return View(ViewModel);
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
                ProductFilterId=product.ProductFilterId,

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

            var viewModel = new ProductFilterViewModel
            {
                Products = results.ToList(),

                // علشان تحافظ على الفلتر الجانبي ظاهر حتى في حالة البحث
                Categories = _categoryRepo.GetAll()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),

                ProductFilters = _productFilterRepo.GetAll()
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Size }).ToList(),

                SelectedCategoryId = null,
                SelectedProductFilterId = null
            };

            if (results == null || !results.Any())
            {
                return View("NoResults", query);
            }

            return View("Index", viewModel);
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
