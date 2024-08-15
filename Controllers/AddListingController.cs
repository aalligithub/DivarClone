using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivarClone.Controllers
{
    [Authorize]
    public class AddListingController : Controller
    {
        private readonly ILogger<AddListingController> _logger;
        private readonly DivarCloneContext _context;

        public AddListingController(DivarCloneContext context, ILogger<AddListingController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing)
        {
            if (ModelState.IsValid)
            {
                listing.DateTimeOfPosting = DateTime.Now;

                _context.Listings.Add(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Listing"); // Redirect to the Listing Index page after successful creation
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }
           
            // If the model is invalid or an exception occurred, return the same view with the model and error information
            return View("Index", listing);
        }
    }
}
