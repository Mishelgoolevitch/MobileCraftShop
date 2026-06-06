using Microsoft.EntityFrameworkCore;
using MobileCraftShop.Data;
using MobileCraftShop.Models;
using MobileCraftShop.ViewModels;

namespace MobileCraftShop.Services
{
    public interface IProductService
    {
        // Асинхронно получает отфильтрованный, отсортированный и разбитый на страницы список продуктов
        Task<ProductListViewModel> GetProductsAsync(ProductListViewModel filter);

        // Выбирает один продукт по его первичному ключу
        Task<Product?> GetProductByIdAsync(int id);

        // Выводит список похожих товаров в зависимости от категории или бренда
        // По умолчанию для стандартной строки пользовательского интерфейса установлено значение 4
        Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 4);

        Task<List<Product>> GetFeaturedProductsAsync(int count = 8);
        Task<List<Product>> GetNewArrivalsAsync(int count = 8);
        Task<List<Product>> GetBestsellersAsync(int count = 8);
        Task<List<Brand>> GetBrandsAsync(int count = 6);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetBestsellersAsync(int count = 8)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                .Where(p => p.IsBestseller && p.IsActive)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Brand>> GetBrandsAsync(int count = 6)
        {
            return await _context.Brands.Where(b => b.IsActive == true).Take(count).ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProductsAsync(int count = 8)
        {
            return await _context.Products
                 .Include(p => p.Brand)
                 .Include(p => p.Reviews)
                 .Where(p => p.IsFeatured && p.IsActive)
                 .Take(count)
                 .ToListAsync();
        }

        public async Task<List<Product>> GetNewArrivalsAsync(int count = 8)
        {
            return await _context.Products
                 .Include(p => p.Brand)
                 .Include(p => p.Reviews)
                 .Where(p => p.IsNewArrival && p.IsActive)
                 .OrderByDescending(p => p.CreatedAt)
                 .Take(count)
                 .ToListAsync();
        }

       
        public async Task<ProductListViewModel> GetProductsAsync(ProductListViewModel filter)
        {
            // 1. Инициализировать запрос связанными данными
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Where(p => p.IsActive)
                .AsQueryable();

            // 2. Применение динамических фильтров
            if (filter.SelectedCategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.SelectedCategoryId.Value);
            if (filter.SelectedBrandId.HasValue)

                query = query.Where(p => p.BrandId == filter.SelectedBrandId.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(p => p.Name.Contains(filter.SearchTerm) ||
                                         p.Description.Contains(filter.SearchTerm) ||
                                         p.Brand.Name.Contains(filter.SearchTerm));

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.SalePrice >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.SalePrice <= filter.MaxPrice.Value);

            // 3. Применить сортировку с помощью выражения Switch на C#
            query = filter.SortOrder?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.SalePrice),
                "price_desc" => query.OrderByDescending(p => p.SalePrice),
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            // 4. Рассчитать статистику разбивки на страницы
            filter.TotalCount = await query.CountAsync();
            filter.TotalPages = (int)Math.Ceiling(filter.TotalCount / (double)filter.PageSize);

            // 5. Выполнить запрос с пропуском 
            filter.Products = await query
        .Skip((filter.PageNumber - 1) * filter.PageSize)
        .Take(filter.PageSize)
        .ToListAsync();

            // 6. Заполните выпадающие списки фильтров
            filter.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            filter.Brands = await _context.Brands.Where(b => b.IsActive).ToListAsync();

            return filter;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            // Извлекает продукт со всеми связанными с ним деталями
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User) // Глубокая загрузка пользователя, написавшего отзыв
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        }


        public async Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 4)
        {
            // 1. Выберите базовый продукт, чтобы узнать его марку и категорию
            var product = await _context.Products.FindAsync(productId);

            // Возвращает пустой список, если продукт не существует
            if (product == null) return new List<Product>();

            // 2. Запросите похожие продукты
            return await _context.Products
                .Include(p => p.Brand)
                .Where(p => p.Id != productId &&      // Исключить текущий продукт
                            (p.BrandId == product.BrandId || p.CategoryId == product.CategoryId) &&
                            p.IsActive)               // Показывать только активные элементы
                .OrderBy(x => Guid.NewGuid())        // Необязательно: Рандомизируйте результаты
                .Take(count)                          // Ограничьте результирующий набор
                .ToListAsync();
        }
    }
}
