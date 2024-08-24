using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace DivarClone.Controllers
{
    
    [Authorize]
    public class AddListingController : Controller
    {
        private readonly ILogger<AddListingController> _logger;
        private readonly DivarCloneContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IListingService _service;

        public AddListingController(DivarCloneContext context, ILogger<AddListingController> logger, IWebHostEnvironment webHostEnvironment, IListingService service)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FormPartial()
        {
            return PartialView("_AddListingForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]     
        public async Task<IActionResult> Create(Listing listing, IFormFile? ImageFile)
        {
            bool imageProcessed = await _service.ProcessImageAsync(listing, ImageFile);
            if (!imageProcessed)
            {
                ModelState.AddModelError("", "Image processing error. ");
            }

            if (ModelState.IsValid)
            {
                bool createlisting = await _service.CreateListingAsync(listing);
                if (!createlisting)
                {
                    ModelState.AddModelError("", "Image processing error. ");
                }
                else { 
                    return RedirectToAction("Index", "Home");
                }                              
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }
            return View("Index", listing);
        }

        public IActionResult EditListing(int id)
        {
            var listing = _service.GetSpecificListing(id);
            return View("EditListing", listing);
        }
    }
}
