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
    }
}
