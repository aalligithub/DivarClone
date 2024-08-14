using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Areas.Identity.Data;  // Add this to use DivarCloneContext

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
