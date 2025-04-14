
using Ecommerce_Website.Data;

namespace Ecommerce_Website.Repositories
{
    public class BorshorRepository : IRepository<Borshor>
    {
        private readonly ApplicationDbContext _context;

        public BorshorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Borshor entity)
        {
            _context.Borshors.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var borshor = _context.Borshors.Find(id);
            if (borshor != null)
            {
                _context.Borshors.Remove(borshor);
                _context.SaveChanges(); // ✅ مهم جدًا
            }
        }

        public IEnumerable<Borshor> GetAll()
        {
            return _context.Borshors.ToList();
        }

        public Borshor GetById(int id)
        {
            return _context.Borshors.Find(id);
        }

        public void Update(Borshor entity)
        {
            _context.Borshors.Update(entity);
            _context.SaveChanges();
        }
    }
}
