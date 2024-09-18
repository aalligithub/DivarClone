using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace DivarClone.Controllers
{

    [Authorize(Roles = "Admin, User")]
    public class AddListingController : Controller
    {
        private readonly ILogger<AddListingController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IListingService _service;

        public AddListingController(ILogger<AddListingController> logger, IWebHostEnvironment webHostEnvironment, IListingService service)
        {
            _logger = logger;
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
                    ModelState.AddModelError("", "Data integrity error. ");
                }
                else
                {
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

        [Authorize]
        public IActionResult EditListing(int id)
        {
            var listing = _service.GetSpecificListing(id);
            if (listing != null)
            {
                return View("EditListing", listing);
            }
            else { return null; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Listing listing, IFormFile? ImageFile)
        {
            bool imageProcessed = await _service.ProcessImageAsync(listing, ImageFile);
            if (!imageProcessed)
            {
                ModelState.AddModelError("", "Image processing error. ");
            }

            if (ModelState.IsValid)
            {
                bool updateSuccess = await _service.UpdateListingAsync(listing);
                if (!updateSuccess)
                {
                    ModelState.AddModelError("", "Image processing error. ");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
