using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]

        public string CustomerName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]

        [Phone(ErrorMessage = "رقم هاتف غير صحيح")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]

        public string CustomerAddress { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
