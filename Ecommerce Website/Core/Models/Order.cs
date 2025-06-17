namespace Ecommerce_Website.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
