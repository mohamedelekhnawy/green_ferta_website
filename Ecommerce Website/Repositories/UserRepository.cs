
using Ecommerce_Website.Data;

namespace Ecommerce_Website.Repositories
{
    public class UserRepository : IRepository<ApplicationUser>

    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ApplicationUser entity)
        {
            _context.Users.Add(entity);
            
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();    
            }
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(ApplicationUser entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
