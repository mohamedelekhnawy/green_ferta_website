using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.ViewModels
{
    public class CategoriesViewModel

    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Max leangh can't be mor than 100 chr.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; } 
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
