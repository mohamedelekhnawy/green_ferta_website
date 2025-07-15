using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Ecommerce_Website.Controllers
{
    [Authorize (Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IOrderRepository _orderRepo;


        public DashboardController(IRepository<ApplicationUser> userRepository, IOrderRepository orderRepo)
        {

            _userRepository = userRepository;
            _orderRepo = orderRepo;
        }
        public IActionResult Index()
        {

            var model = new DashboardViewModel
            {
                TotalOrdersCount = _orderRepo.GetTotalOrdersCount(),
                TodayOrdersCount = _orderRepo.GetTodayOrdersCount(),
                TotalRevenue = _orderRepo.GetTotalRevenue(),
                TodayRevenue = _orderRepo.GetTodayRevenue(),
                RecentOrders = _orderRepo.GetRecentOrders(7)
            };

            return View(model);
        }
        public IActionResult Users()
        {
            var users = _userRepository.GetAll();

            var viewModelList = users.Select(user => new UsersViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
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
