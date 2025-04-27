using Ecommerce_Website.Repositories;
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
        
        [HttpPost]
        public IActionResult Testmonials(Testmonials model)
        {

            var viewModel = new TestmonialsViewModel()
            {
                Name= model.Name,
                Message= model.Message,
                CreatedAt= DateTime.Now,
            };
            _testmonialsRepository.Add(model);
            return RedirectToAction("Index");
        }
    }
}
