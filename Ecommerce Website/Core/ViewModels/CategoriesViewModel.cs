using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class CategoriesViewModel
    {
        [MaxLength(100, ErrorMessage = "Max leangh can't be mor than 100 chr.")]
        
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

    }
}
