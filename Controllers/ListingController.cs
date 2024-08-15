using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Areas.Identity.Data;  // Add this to use DivarCloneContext

// خود لیستینگ هم بصورت صفحه مجزا و خارج از خانه با url خودش قابل نمایش است
namespace DivarClone.Controllers
{
    public class ListingController : Controller
    {
        private readonly DivarCloneContext _context;

        public ListingController(DivarCloneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve all listings from the database
            var listings = _context.Listings.ToList();
            return View(listings);
        }
    }
}
