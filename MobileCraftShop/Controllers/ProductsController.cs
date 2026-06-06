using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Services;
using MobileCraftShop.ViewModels;

namespace MobileCraftShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;

        // Внедрение продукта сервиса через конструктор
        public ProductsController(IProductService productService, IShoppingCartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
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
            ViewBag.CartItemCount = await _cartService.GetCartItemCountAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Json(new List<object>());

            var filter = new ProductListViewModel
            {
                SearchTerm = term,
                PageSize = 5
            };

            var result = await _productService.GetProductsAsync(filter);
            var suggestions = result.Products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.SalePrice,
                image = p.MainImageUrl,
                brand = p.Brand?.Name
            });

            return Json(suggestions);
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
            ViewBag.CartItemCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }
    }
}
