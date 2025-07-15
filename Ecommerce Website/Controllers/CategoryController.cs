using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly int _MaxSize = 11242880;

        private readonly IRepository<CategoryModel> _categoryRepo;

        public CategoryController(IRepository<CategoryModel> categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Categories()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit (int id)
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
                ImageUrl = category.ImageUrl,

            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepo.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            // حذف الصورة من wwwroot لو فيه ImageUrl
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Categories", category.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _categoryRepo.Delete(id);

            return RedirectToAction("Categories");
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
                ImageUrl = category.ImageUrl,
                Description = category.Description
            };

            return View("CategoryDetails", viewModel);
        }
        

    }
}
