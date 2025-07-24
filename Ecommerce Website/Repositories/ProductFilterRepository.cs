
namespace Ecommerce_Website.Repositories
{
    public class ProductFilterRepository : IRepository<ProductFilter>
    {
        private readonly ApplicationDbContext _context;

        public ProductFilterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProductFilter entity)
        {
            _context.ProductFilters.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.ProductFilters.Remove(GetById(id));
            _context.SaveChanges();
        }

        public IEnumerable<ProductFilter> GetAll()
        {
            return _context.ProductFilters.ToList();
        }

        public ProductFilter GetById(int id)
        {
            return _context.ProductFilters.FirstOrDefault(pf => pf.Id == id) 
                   ?? throw new KeyNotFoundException($"ProductFilter with ID {id} not found.");
        }

        public void Update(ProductFilter entity)
        {
            _context.ProductFilters.Update(entity);
            _context.SaveChanges();
        }
    }
}
