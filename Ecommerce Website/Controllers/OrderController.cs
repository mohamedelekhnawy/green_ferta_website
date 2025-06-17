using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ecommerce_Website.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            // تأكد من أنك تستخدم IQueryable وليس IEnumerable
            var orders = _orderRepository.GetAll()
                .AsQueryable() // تحويل إلى IQueryable إذا لم يكن كذلك
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product) // إذا كنت تحتاج بيانات المنتج
                .Where(o => o != null)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        public IActionResult Add()
        {
            // قراءة السلة من السيشن
            var cartJson = HttpContext.Session.GetString("Cart");
            var cartItems = string.IsNullOrEmpty(cartJson)
                            ? new List<CartItem>()
                            : JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;

            var vm = new OrderViewModel
            {
                Items = cartItems
            };

            return View(vm);
        }

        // POST: Order/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(OrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var order = new Order
            {
                CustomerName = vm.CustomerName,
                CustomerPhone = vm.CustomerPhone,
                CustomerAddress = vm.CustomerAddress,
                CreatedAt = DateTime.Now,
                Items = vm.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    DiscountedPrice = item.DiscountedPrice
                }).ToList()
            };

            _orderRepository.Add(order);

            // تنظيف السلة
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }

        public IActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
