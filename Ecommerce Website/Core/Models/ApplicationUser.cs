

namespace Ecommerce_Website.Core.Models
{
    public class ApplicationUser :IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string ? CreatedById { get; set; }
        public string? UpdatedById { get; set; }
    }
}
