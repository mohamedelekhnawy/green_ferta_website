using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce_Website.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public CartController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cartItems = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            // التحقق من الكمية
            if (quantity <= 0)
            {
                TempData["Error"] = "الكمية يجب أن تكون أكبر من الصفر.";
                return RedirectToAction("Index", "Home");
            }

            var product = _productRepository.GetById(productId);
            if (product == null || !product.IsAvailable)
            {
                TempData["Error"] = "المنتج غير متاح.";
                return RedirectToAction("Index", "Home");
            }

            // استرجاع السلة الحالية
            var cartJson = HttpContext.Session.GetString("Cart");
            var cartItems = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;

            // التحقق من وجود المنتج مسبقًا
            var existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > 100)
                {
                    TempData["Error"] = "لا يمكن تجاوز الحد الأقصى للكمية (100).";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    DiscountedPrice = product.DiscountedPrice,
                    Quantity = quantity
                });
            }

            // حفظ السلة
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
            TempData["Success"] = "تمت إضافة المنتج إلى السلة بنجاح!";

            // العودة للصفحة السابقة
            string referer = Request.Headers["Referer"].ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
        }


        [HttpPost]
        public IActionResult UpdateCart(List<CartItemUpdateModel> items)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cartItems = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;

            foreach (var updateItem in items)
            {
                var existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == updateItem.ProductId);
                if (existingItem != null)
                {
                    if (updateItem.Quantity <= 0)
                    {
                        cartItems.Remove(existingItem);
                    }
                    else if (updateItem.Quantity > 100)
                    {
                        TempData["Error"] = "لا يمكن أن تزيد الكمية عن 100 لكل منتج";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        existingItem.Quantity = updateItem.Quantity;
                    }
                }
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
            TempData["Success"] = "تم تحديث السلة بنجاح";
            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int productId)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;
                var itemToRemove = cartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
                    TempData["Success"] = "تم إزالة المنتج من السلة";
                }
            }

            return RedirectToAction("Index");
        }
    }

    public class CartItemUpdateModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

