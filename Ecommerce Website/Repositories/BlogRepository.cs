


namespace Ecommerce_Website.Repositories
{
    public class BlogRepository : IRepository<Blog>
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Blog entity)
        {
            _context.Blogs.Add(entity);
            _context.SaveChanges(); 
        }

        public void Delete(int id)
        {
            var blogs = _context.Blogs.Find(id);
            if (blogs != null)
            {
                _context.Blogs.Remove(blogs);
                _context.SaveChanges(); // ✅ مهم جدًا
            }
        }

        public IEnumerable<Blog> GetAll()
        {
            return _context.Blogs.ToList();

        }

        public Blog GetById(int id)
        {
            return _context.Blogs.Find(id);
        }

        public void Update(Blog entity)
        {
            _context.Blogs.Update(entity);
            _context.SaveChanges();
        }
    }
}
