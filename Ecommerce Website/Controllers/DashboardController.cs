using Ecommerce_Website.Core.Models;
using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
namespace Ecommerce_Website.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IRepository<CategoryModel> _categoryRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Borshor> _borshorRepo;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly List<string> _PdfallowedExtensions = new List<string> { ".pdf" };
        private readonly int _MaxSize= 5242880 ;
        

        public DashboardController(IRepository<CategoryModel> categoryRepo, IRepository<Product> productRepo, IWebHostEnvironment webHostEnvironment, IRepository<Borshor> borshorRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _webHostEnvironment = webHostEnvironment;
            _borshorRepo = borshorRepo;
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

            var category= new CategoryModel
            {
                Name = model.Name,
                Description = model.Description, 
                CreatedOn = DateTime.Now
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
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Categories", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                category.ImageUrl = imageName;
            }

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
            var imageUrl = string.IsNullOrEmpty(category.ImageUrl) ? "/Images/Products/default.png" : category.ImageUrl;
            var viewModel = new CategoriesViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl= category.ImageUrl,
                
            };

            return View("EditForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoriesViewModel model, IFormFile? Image)
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
            category.ImageUrl = model.ImageUrl;
            category.Description = model.Description;
            category.UpdatedOn = DateTime.Now;

            if (Image != null && Image.Length > 0)
            {

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Categories");
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
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, Path.GetFileName(category.ImageUrl));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // تحديث الصورة في قاعدة البيانات
                category.ImageUrl = uniqueFileName;
            }

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


        // ✅ عرض فورم إضافة منتج جديد
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

        // ✅ إضافة منتج جديد مع حفظ صورة
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

        [HttpGet]
        public IActionResult EditProduct(int id)
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
                    .ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(ProductsViewModel model, IFormFile? Image)
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
                product.ImageUrl =   uniqueFileName;
            }

            _productRepo.Update(product);

            return RedirectToAction("product");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
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
        [HttpGet]
        public IActionResult ViewProduct(int Id)
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
                CategoryName = product.Category?.Name,
                HowToUse = product.HowToUse,
                IsAvailable = product.IsAvailable,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
            return View(ViewModel);
        }

        public IActionResult Borshors()
        {
            var Borshors = _borshorRepo.GetAll();
            return View(Borshors);
        }

        public IActionResult AddBorshor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBorshor(BorshorViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var borshor = new Borshor
            {
                Titel = model.Titel,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            if (model.Image !=null) {

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
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Borshors", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                borshor.ImageUrl = imageName;
            }
            if (model.Pdf != null)
            {
                var extension = Path.GetExtension(model.Pdf.FileName).ToLower();
                if (! _PdfallowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Pdf), "Not allowed extension");
                    return View(model);
                }
                var pdfName = $"{Guid.NewGuid()}{extension}";
                var pdfPath = Path.Combine(_webHostEnvironment.WebRootPath, "Pdf/Borshors", pdfName);
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    model.Pdf.CopyTo(stream);
                }
                borshor.PdfUrl = pdfName;
            }
            _borshorRepo.Add(borshor);
            return RedirectToAction("Borshors");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBorshor(int id)
        {
            var borshor = _borshorRepo.GetById(id);

            if (borshor == null)
            {
                return NotFound();
            }

            // Optional: Delete the image from wwwroot if it exists
            if (!string.IsNullOrEmpty(borshor.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Borshors", borshor.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            if (!string.IsNullOrEmpty(borshor.PdfUrl))
            {
                var PdfPath = Path.Combine(_webHostEnvironment.WebRootPath, "Pdf/Borshors", borshor.PdfUrl);
                if (System.IO.File.Exists(PdfPath))
                {
                    System.IO.File.Delete(PdfPath);
                }
            }

            // Delete the product
            _borshorRepo.Delete(id);
            return RedirectToAction("Borshors"); // Redirect after deletion
        }

    }
}
