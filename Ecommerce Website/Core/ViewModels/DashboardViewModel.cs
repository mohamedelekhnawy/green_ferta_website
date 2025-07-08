namespace Ecommerce_Website.Core.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrdersCount { get; set; }
        public int TodayOrdersCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public IEnumerable<Order> RecentOrders { get; set; } = new List<Order>();
    }
}
