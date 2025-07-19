using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    
    public class AboutUsController : Controller
    {
        private readonly IRepository<Testmonials> _testmonialsRepository;

        public AboutUsController(IRepository<Testmonials> testmonialsRepository)
        {
            _testmonialsRepository = testmonialsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Testmonials()
        {
            var testmonials = _testmonialsRepository.GetAll();
            return View(testmonials);
        }

        [HttpPost]
        public IActionResult Testmonials(Testmonials model)
        {

            var viewModel = new TestmonialsViewModel()
            {
                Name = model.Name,
                Message = model.Message,
                CreatedAt = DateTime.Now,
            };
            _testmonialsRepository.Add(model);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var testmonials = _testmonialsRepository.GetById(id);
            if (testmonials == null)
            {
                return NotFound();
            }
            _testmonialsRepository.Delete(id);
            return RedirectToAction("Testmonials");
        }
    }
}
