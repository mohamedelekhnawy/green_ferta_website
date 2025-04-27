
using Ecommerce_Website.Data;

namespace Ecommerce_Website.Repositories
{
    public class TestmonialsRepository : IRepository<Testmonials>
    {
        private readonly ApplicationDbContext _context;

        public TestmonialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Testmonials entity)
        {
            _context.Testmonials.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var testmonial= _context.Testmonials.Find(id);
            _context.Remove(testmonial);
            _context.SaveChanges();
        }

        public IEnumerable<Testmonials> GetAll()
        {
           return _context.Testmonials.ToList();
        }

        public Testmonials GetById(int id)
        {
            return _context.Testmonials.Find(id);
        }

        public void Update(Testmonials entity)
        {
            throw new NotImplementedException();
        }
    }
}
