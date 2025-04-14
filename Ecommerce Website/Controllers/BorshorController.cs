using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class BorshorController : Controller
    {
        private readonly IRepository<Borshor> _borshorRepo;

        public BorshorController(IRepository<Borshor> borshorRepo)
        {
            _borshorRepo = borshorRepo;
        }

        public IActionResult Index()
        {
            var borshor = _borshorRepo.GetAll();
            return View(borshor);
        }
    }
}
