using Microsoft.AspNetCore.Mvc;

namespace MobileCraftShop.Controllers
{
    public class CheckoutViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
