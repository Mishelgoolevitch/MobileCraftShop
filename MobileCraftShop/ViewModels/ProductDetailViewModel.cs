using MobileCraftShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MobileCraftShop.ViewModels
{
    public class ProductDetailViewModel
    {
        // Основной просматриваемый продукт
        public Product Product { get; set; } = null!;

        // Список похожих товаров (например, из той же торговой марки или категории)
        public List<Product> RelatedProducts { get; set; } = new List<Product>();

        // Используется для переключения значка сердечка, если пользователь уже сохранил этот элемент
        public bool IsInWishlist { get; set; }
    }

    public class ReviewViewModel
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}
