using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } =string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get
            {
                // استخدام السعر المخفض إذا كان له قيمة (غير null) وأكبر من 0
                // وإلا استخدام السعر العادي
                decimal unitPrice = (DiscountedPrice.HasValue && DiscountedPrice > 0)
                    ? DiscountedPrice.Value
                    : Price;

                return unitPrice * Quantity;
            }
        }
    }
}
