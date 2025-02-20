using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!; 

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = null!; 

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } 

        [StringLength(1000)]
        public string? Comment { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
