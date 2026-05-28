using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Models;
using MobileCraftShop.Services;
using System.Diagnostics;

namespace MobileCraftShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductService productService, ILogger<HomeController> logger)
        {
            _productService = productService;
            _logger = logger;
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
