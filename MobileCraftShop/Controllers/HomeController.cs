using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Models;
using MobileCraftShop.Services;
using System.Diagnostics;

namespace MobileCraftShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductService productService, ILogger<HomeController> logger, IShoppingCartService cartService)
        {
            _productService = productService;
            _logger = logger;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _productService.GetFeaturedProductsAsync(8),
                NewArrivals = await _productService.GetNewArrivalsAsync(8),
                Bestsellers = await _productService.GetBestsellersAsync(8),
                Brands = await _productService.GetBrandsAsync(6)
            };
            ViewBag.CartItemCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public List<Product> NewArrivals { get; set; } = new List<Product>();
        public List<Product> Bestsellers { get; set; } = new List<Product>();
        public List<Brand> Brands { get; set; } = new List<Brand>();
    }
}
