using MobileCraftShop.Models;

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
}
