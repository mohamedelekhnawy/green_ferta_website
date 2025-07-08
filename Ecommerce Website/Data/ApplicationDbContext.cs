
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ecommerce_Website.Core.ViewModels;

namespace Ecommerce_Website.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> orders { get; set; }
        public object Orders { get; internal set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Borshor> Borshors { get; set; }
        public DbSet<Testmonials> Testmonials { get; set; }
        public DbSet<Blog> Blogs{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) 
                .WithMany(c => c.Products) 
                .HasForeignKey(p => p.CategoryId);

        }
    }
}
