namespace Ecommerce_Website.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByCategoryId(int categoryId);
        IEnumerable<CategoryModel> GetAllCategories();
        IEnumerable<Product> Search(string query);
    }
}
