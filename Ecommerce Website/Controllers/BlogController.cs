using Ecommerce_Website.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<Blog> _blogrepo;

        public BlogController(IRepository<Blog> blogrepo)
        {
            _blogrepo = blogrepo;
        }

        public IActionResult Index()
        {
            var blogs = _blogrepo.GetAll();

            var viewModel = blogs.Select(b => new BlogsViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                ImageUrl = b.ImageUrl,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
                // زود باقي الخصائص لو فيه
            }).ToList();

            return View(viewModel);

        }
        public IActionResult BlogDetails(int id)
        {
            var blog = _blogrepo.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            var viewModel = new BlogsViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
            return View("BlogDetails", viewModel);

        }
    }
}
