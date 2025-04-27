namespace Ecommerce_Website.Core.ViewModels
{
    public class UsersViewModel
    {
        
        public string Id { get; set; }= null !;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;        
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ? UpdatedAt { get; set; }
    }
}
