
using Ecommerce_Website.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website.Repositories
{
    public class OrdersRepository : IRepository<Order>
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
            var orders = _context.Products.Find(id);
            if (orders != null)
            {
                _context.Products.Remove(orders);
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
            return _context.orders.Find(id);
        }

        public void Update(Order entity)
        {
            _context.orders.Update(entity);
            _context.SaveChanges();
        }
    }
}
