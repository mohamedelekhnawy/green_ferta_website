using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly int _MaxSize = 11242880;

        private readonly IRepository<Blog> _blogrepo;

        public BlogController(IRepository<Blog> blogrepo, IWebHostEnvironment webHostEnvironment)
        {
            _blogrepo = blogrepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var blogs = _blogrepo.GetAll();

            var viewModel = blogs.Select(b => new BlogsViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                ImageUrl = b.ImageUrl,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
                // زود باقي الخصائص لو فيه
            }).ToList();

            return View(viewModel);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Blogs()
        {
            var blogs = _blogrepo.GetAll();
            return View(blogs);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(BlogsViewModel model)
        {

            if (ModelState.IsValid)
            {
                return View(model);
            }
            var blog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
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
                    ModelState.AddModelError(nameof(model.Image), "Maximum size is 10MB");
                    return View(model);
                }
                var imageName = $"{Guid.NewGuid()}{extension}";
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Blogs", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                blog.ImageUrl = imageName;
            }
            _blogrepo.Add(blog);
            return RedirectToAction(nameof(Blogs));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var blog = _blogrepo.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }

            var viewModel = new BlogsViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogsViewModel model, IFormFile? Image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blog = _blogrepo.GetById(model.Id);
            if (blog == null)
            {
                return NotFound();
            }

            // تحديث البيانات الأساسية
            blog.Title = model.Title;
            blog.Content = model.Content;
            blog.UpdatedAt = DateTime.UtcNow;

            // معالجة الصورة الجديدة
            if (Image != null && Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Blogs");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // حفظ الصورة الجديدة
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string newFilePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                // حذف الصورة القديمة لو كانت موجودة
                if (!string.IsNullOrEmpty(blog.ImageUrl))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, blog.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                blog.ImageUrl = newFileName;
            }
            else
            {
                // لو مفيش صورة جديدة، خليه يحتفظ بالقديمة
                blog.ImageUrl = model.ImageUrl;
            }

            _blogrepo.Update(blog);

            return RedirectToAction("Blogs");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var blog = _blogrepo.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            // Optional: Delete the image from wwwroot if it exists
            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Blogs", blog.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            // Delete the blog
            _blogrepo.Delete(id);
            return RedirectToAction("Blogs"); // Redirect after deletion
        }
        public IActionResult BlogDetails(int id)
        {
            var blog = _blogrepo.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            var viewModel = new BlogsViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
            return View("BlogDetails", viewModel);

        }
    }
}
