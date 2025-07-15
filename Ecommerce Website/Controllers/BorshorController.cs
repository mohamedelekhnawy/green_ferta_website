using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class BorshorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly List<string> _PdfallowedExtensions = new List<string> { ".pdf" };
        private readonly int _MaxSize = 11242880;

        private readonly IRepository<Borshor> _borshorRepo;

        public BorshorController(IRepository<Borshor> borshorRepo, IWebHostEnvironment webHostEnvironment)
        {
            _borshorRepo = borshorRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var borshor = _borshorRepo.GetAll();
            return View(borshor);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Borshors()
        {
            var Borshors = _borshorRepo.GetAll();
            return View(Borshors);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(BorshorViewModel model)
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
                if (!_PdfallowedExtensions.Contains(extension))
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
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
