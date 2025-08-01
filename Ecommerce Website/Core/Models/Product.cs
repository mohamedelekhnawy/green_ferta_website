﻿

using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class Product : Base
    {
        public int Id { get; set; }

        [Required]
        
        public string Name { get; set; } = null!;

        [Required]
        
        public string? Description { get; set; }

        
        public string? Importance { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountedPrice { get; set; }

        public string? ImageUrl { get; set; }

        // Category
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual CategoryModel? Category { get; set; }

        // How to Use instead of old Quantity
        
        public string? HowToUse { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Product Filter
        public int? ProductFilterId { get; set; }

        // Navigation Property
        public virtual ProductFilter? ProductFilter { get; set; }
    }
}
