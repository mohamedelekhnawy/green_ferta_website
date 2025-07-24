using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website.Core.Models
{
    public class ProductFilter
    {
        public int Id { get; set; }

        public string Size { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
