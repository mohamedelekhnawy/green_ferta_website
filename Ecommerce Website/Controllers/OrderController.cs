using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ecommerce_Website.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository ,ILogger<OrderController> logger)
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

        [HttpGet]
        public IActionResult Add()
        {
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
                    _logger.LogWarning("النموذج غير صالح. أخطاء التحقق: {@Errors}",
                        ModelState.Values.SelectMany(v => v.Errors));
                    return View(vm);
                }

                // 4. التحقق من وجود عناصر في السلة
                if (!vm.Items.Any())
                {
                    _logger.LogWarning("محاولة إتمام طلب بسلة فارغة");
                    ModelState.AddModelError("", "السلة فارغة. لا يمكن إتمام الطلب بدون منتجات.");
                    return View(vm);
                }

                // 5. إنشاء الطلب
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
