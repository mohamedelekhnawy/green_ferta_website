using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        [Required]
        
        public string Name { get; set; } = null!;
        [Required]
        
        public string? Description { get; set; }
        

        
        public string? Importance { get; set; }

        [Required]
        [Range(0.00, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0.00, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountedPrice { get; set; }
        
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        //Category
        [Required]
        public int SelectedCategoryId { get; set; }
        public string? CategoryName { get; set; }
        [Required]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        //-------------------//

        public string? HowToUse { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
