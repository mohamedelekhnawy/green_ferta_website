
using Ecommerce_Website.Data;

namespace Ecommerce_Website.Repositories

{
    public class CategoryRepository : IRepository<CategoryModel>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<CategoryModel> GetAll()
        {
            return _context.Categories.ToList();
        }

        public CategoryModel GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public void Add(CategoryModel entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CategoryModel entity)
        {
            _context.Categories.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
