using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class ProductsViewModel
    {

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

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


        public virtual CategoryModel? Category { get; set; }
        public string Quantity { get; set; } = null!;

        public bool IsAvilable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<SelectListItem> Categories { get; set; } =null!;

        [Required]
        [ForeignKey("Category")]
        public int SelectedCategoryId { get; set; }
    }
}
