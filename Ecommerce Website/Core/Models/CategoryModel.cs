namespace Ecommerce_Website.Core.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; } 
        public DateTime? UpdatedOn { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
