using Ecommerce_Website.Data;
using System;

namespace Ecommerce_Website.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetByCustomer(string customerName);
        int GetTotalOrdersCount();
        int GetTodayOrdersCount();
        decimal GetTotalRevenue();
        decimal GetTodayRevenue();
        IEnumerable<Order> GetRecentOrders(int days = 7);

    }
}
