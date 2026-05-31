using Microsoft.AspNetCore.Mvc;
using MobileCraftShop.Data;

namespace MobileCraftShop.ViewComponents
{
    public class BrandsMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BrandsMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            
            var brands = _context.Brands
                                 .Where(b => b.IsActive)
                                 .OrderBy(b => b.Name)
                                 .ToList();

            return View(brands);
        }
    }
}
