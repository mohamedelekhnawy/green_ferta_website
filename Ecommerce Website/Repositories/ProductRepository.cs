
using Ecommerce_Website.Data;
using Ecommerce_Website.Repositories;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.ToList();
    }

    public Product GetById(int id)
    {
        return _context.Products.Find(id);
    }

    public void Add(Product entity)
    {
        _context.Products.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Product entity)
    {
        _context.Products.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
    {
        return _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToList();
    }

    public IEnumerable<CategoryModel> GetAllCategories()
    {
        return _context.Categories.ToList();
    }
    public IEnumerable<Product> Search(string query)
    {
        return _context.Products
            .Include(p => p.Category)
            .Where(p =>
                p.Name.Contains(query) ||
                p.Description.Contains(query) ||
                p.Category.Name.Contains(query) ||
                p.Category.Description.Contains(query))
            .ToList();
    }
}
