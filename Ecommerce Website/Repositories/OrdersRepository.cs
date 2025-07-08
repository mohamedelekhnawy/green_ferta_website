using Ecommerce_Website.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website.Repositories
{
    public class OrdersRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Order entity)
        {
            _context.orders.Add(entity);
            _context.SaveChanges();
        }


        public void Delete(int id)
        {
            var order = _context.orders.Find(id);
            if (order != null)
            {
                _context.orders.Remove(order);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Order> GetAll()
        {
            return _context.orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

        }
        public Order GetById(int id)
        {
            return _context.orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id);
        }
        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Order> GetByCustomer(string customerName)
        {
            return _context.orders
                .Where(o => o.CustomerName.Contains(customerName))
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToList();
        }

        // 2. عدد الطلبات الكلي
        public int GetTotalOrdersCount()
        {
            return _context.orders.Count();
        }

        // 3. عدد طلبات اليوم
        public int GetTodayOrdersCount()
        {
            var today = DateTime.Today;
            return _context.orders
                .Count(o => o.CreatedAt.Date == today);
        }

        // 4. إجمالي الإيرادات
        public decimal GetTotalRevenue()
        {
            return _context.orders
                .SelectMany(o => o.Items)
                .Sum(i => (i.DiscountedPrice ?? i.Price) * i.Quantity);
        }

        // 5. إيرادات اليوم
        public decimal GetTodayRevenue()
        {
            var today = DateTime.Today;
            return _context.orders
                .Where(o => o.CreatedAt.Date == today)
                .SelectMany(o => o.Items)
                .Sum(i => (i.DiscountedPrice ?? i.Price) * i.Quantity);
        }

        // 6. أحدث الطلبات (اختياري بعدد الأيام)
        public IEnumerable<Order> GetRecentOrders(int days = 7)
        {
            var fromDate = DateTime.Now.AddDays(-days);
            return _context.orders
                .Where(o => o.CreatedAt >= fromDate)
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToList();
        }
    }
}
