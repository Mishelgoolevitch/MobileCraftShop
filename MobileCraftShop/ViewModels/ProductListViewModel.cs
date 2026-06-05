using MobileCraftShop.Models;

namespace MobileCraftShop.ViewModels
{
    public class ProductListViewModel
    {
        // Списки данных
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Brand> Brands { get; set; } = new List<Brand>();

        // Фильтр параметров
        public int? SelectedCategoryId { get; set; }
        public int? SelectedBrandId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }

        // Фильтрация ценового диапазона
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Свойства разбивки на страницы
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
