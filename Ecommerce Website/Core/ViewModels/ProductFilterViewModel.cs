using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class ProductFilterViewModel
    {
        public string ProductSize { get; set; } = null!;
        public int? SelectedCategoryId { get; set; }
        public int? SelectedProductFilterId { get; set; }

        // القوائم المعروضة في الـ Dropdowns
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> ProductFilters { get; set; } = new();

        // قائمة المنتجات المعروضة فعليًا
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

    }
}
