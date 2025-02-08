

using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class Product : Base
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }=null!;

        [Required]
        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? Importance { get; set; } 

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountedPrice { get; set; }

        public string? ImageUrl { get; set; }
        
        public List<string>? ImageUrls { get; set; } = new List<string>();

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual CategoryModel? Category { get; set; }

        [Required]
        [StringLength(50)]
        public string ?SKU { get; set; } 

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }
        public bool IsActive { get; set; } = true;


        [StringLength(200)]
        public string? Slug { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        
    }
}
