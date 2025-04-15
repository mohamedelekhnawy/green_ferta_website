using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<ApplicationUser> _userRepository;

        public UserController(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAll();

            var viewModelList = users.Select(user => new UsersViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            }).ToList();

            return View(viewModelList);
        }

    }
}
