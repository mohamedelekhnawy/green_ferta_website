using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } // الربط بالمنتج

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } // اسم العميل

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // التقييم من 1 إلى 5 نجوم

        [StringLength(1000)]
        public string? Comment { get; set; } // تعليق العميل (اختياري)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // وقت كتابة التقييم
    }
}
