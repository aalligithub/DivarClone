using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
        public async Task<IActionResult> Create(Listing listing, List<IFormFile>? ImageFiles)
        {
            int? newListingId = null;
            string fileHash = "";

            if (ModelState.IsValid)
            {
                try
                {
                    newListingId = await _service.CreateListingAsync(listing);

                    if (newListingId.HasValue)
                    {
                        var uniqueFiles = new List<(IFormFile File, string Hash)>();
                        var fileHashes = new HashSet<string>();

                        foreach (var ImageFile in ImageFiles)
                        {
                            //Making Images in individual listings distinct
                            try
                            {
                                fileHash = _service.ComputeImageHash(ImageFile.FileName);

                                if (!fileHashes.Contains(fileHash))
                                {
                                    fileHashes.Add(fileHash);
                                    uniqueFiles.Add((ImageFile, fileHash));
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error computing hash for ImageFile");
                                throw;
                            }
                        }

                        if (!uniqueFiles.Any())
                        {
                            _logger.LogTrace("No Image File uploaded going for the default image");
                        }

                        foreach (var (uniqueFile, filehash) in uniqueFiles)
                        {
                            try
                            {
                                await _service.UploadImageToFTP(newListingId, uniqueFile, filehash);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "_service.UploadImageToFTP Image Upload Error ");
                                ModelState.AddModelError(ex.Message, "Image Upload Error ");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AddListingController, Failed to create listing ");
					ModelState.AddModelError(ex.Message, "Failed to create listing");
				}
			}
			var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }
            if (ModelState.ErrorCount == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index", listing);
            }
        }

        [Authorize]
        public IActionResult EditListing(int id)
        {
            var listing = _service.GetSpecificListing(id);
            if (listing != null)
            {
                return View("EditListing", listing);
            }
            else { return NotFound(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Listing listing, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                bool updateSuccess = await _service.UpdateListingAsync(listing);
                if (!updateSuccess)
                {
                    ModelState.AddModelError("", "Error Updating listing ");
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

            var listingToUpdate = _service.GetSpecificListing(listing.Id);
            return View("EditListing", listingToUpdate);

        }
    }
}
