using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Data;
using MobileCraftShop.Models;
using MobileCraftShop.Services;
using Stripe;

namespace MobileCraftShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;  // ADD THIS
        private readonly IShoppingCartService _cartService;

        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,  // ADD THIS PARAMETER)
           IShoppingCartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;  // ADD THIS
            _cartService = cartService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            // 1. Preserving the intended navigation path
            ViewData["ReturnUrl"] = returnUrl;

            // 2. Rendering the onboarding interface
            return View();
        }
    }
}
