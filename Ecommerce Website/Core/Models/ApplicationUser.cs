

namespace Ecommerce_Website.Core.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string ? CreatedById { get; set; }
        public string? UpdatedById { get; set; }
    }
}
