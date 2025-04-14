namespace Ecommerce_Website.Core.ViewModels
{
    public class BorshorViewModel
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }

        //*-------------------*//
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }

        //*-------------------*//
        public IFormFile Pdf { get; set; }
        public string PdfUrl { get; set; }

        //*-------------------*//
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
