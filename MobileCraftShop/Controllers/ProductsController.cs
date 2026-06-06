using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Services;
using MobileCraftShop.ViewModels;

namespace MobileCraftShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        // Внедрение продукта сервиса через конструктор
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Отображает отфильтрованный список товаров с разбивкой по страницам
        /// </summary>
        public async Task<IActionResult> Index(
            int? categoryId,
            int? brandId,
            string? search,
            string? sort,
            decimal? minPrice,
            decimal? maxPrice,
            int page = 1)
        {
            // Сопоставление параметров URL-адреса с нашей моделью просмотра
            var filter = new ProductListViewModel
            {
                SelectedCategoryId = categoryId,
                SelectedBrandId = brandId,
                SearchTerm = search,
                SortOrder = sort,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                PageNumber = page
            };

            // Вызов службы для обработки логики фильтрации
            var result = await _productService.GetProductsAsync(filter);

            return View(result);
        }

        /// <summary>
        /// Отображает подробную информацию о конкретном продукте
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            // 1. Получите основные сведения о продукте из сервиса
            var product = await _productService.GetProductByIdAsync(id);

            // 2. Проверка безопасности: Если продукт не существует или неактивен, верните 404
            if (product == null)
            {
                return NotFound();
            }

            // 3. Поиск сопутствующих товаров (повышение продаж)
            var relatedProducts = await _productService.GetRelatedProductsAsync(id);

            // 4. Заполнитель для логики списка желаний (может быть расширен позже)
            bool isInWishlist = false;

            // 5. Создайте ViewModel
            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts,
                IsInWishlist = isInWishlist
            };

            return View(viewModel);
        }
    }
}
