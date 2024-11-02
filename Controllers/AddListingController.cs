using System.Reflection;
using DivarClone.Attributes;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DivarClone.Controllers
{

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

		[Authorize(Policy = "CreateListingPolicy")]
		[HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FormPartial()
        {
            return PartialView("_AddListingForm");
        }


        //[RoleOrPermissionAuthorize(Permission = "CanCreateListing")]
        [Authorize(Policy = "CreateListingPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing, List<IFormFile>? ImageFiles)
        {
            int? newListingId = null;

            if (ModelState.IsValid)
            {
                try
                {
                    newListingId = await _service.CreateListingAsync(listing);

                    if (newListingId.HasValue)
                    {
                        try {
                            if (await _service.CollectDistinctImages(newListingId, ImageFiles))
                            {
                                TempData["SuccessMessage"] = "آگهی با موفقیت ساخته شد";
                                return RedirectToAction("Index", "Home");
                            }
                        } catch (Exception ex)
                        {
                            _logger.LogError(ex, "AddListingController, CollectDistinctImages, Failed to create listing ");
                            ModelState.AddModelError(ex.Message, "AddListingController, CollectDistinctImages, Failed to make images distinct");
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


		[Authorize(Policy = "ViewSpecialListingPolicy")]
        [HttpGet]
        public IActionResult CreateSecret()
        {
            return View();
        }


		[Authorize(Policy = "ViewSpecialListingPolicy")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSecret(Listing listing, List<IFormFile>? ImageFiles)
        {
            int? newListingId = null;

            if (ModelState.IsValid)
            {
                try
                {
                    newListingId = await _service.CreateListingAsync(listing);

                    if (newListingId.HasValue)
                    {
                        try
                        {
                            if (await _service.CollectDistinctImages(newListingId, ImageFiles))
                            {
                                try
                                {
                                    await _service.MakeListingSecret(newListingId);
                                }
                                catch (Exception ex) {
                                    _logger.LogError(ex, "AddListingController, MakeListingSecret, Failed to make listing secret");
                                    ModelState.AddModelError(ex.Message, "AddListingController, MakeListingSecret, Failed to make listing secret");
                                }

                                TempData["SuccessMessage"] = "آگهی با موفقیت ساخته شد";
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "AddListingController, CollectDistinctImages, Failed to create listing ");
                            ModelState.AddModelError(ex.Message, "AddListingController, CollectDistinctImages, Failed to make images distinct");
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


		[Authorize(Policy = "EditListingPolicy")]
        public IActionResult EditListing(int id)
        {
            var listing = _service.GetSpecificListing(id);
            if (listing != null)
            {
                return View("EditListing", listing);
            }
            else { return NotFound(); }
        }


        [Authorize(Policy = "EditListingPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Listing listing, List<IFormFile?> ImageFiles)
        {
            if (ModelState.IsValid)
            {
                bool updateSuccess = await _service.UpdateListingAsync(listing);
                if (updateSuccess)
                {
                    var newListingId = listing.Id;

                    try
                    {
                        await _service.CollectDistinctImages(newListingId, ImageFiles);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "AddListingController, MakeListingSecret, Failed to make listing secret");
                        ModelState.AddModelError(ex.Message, "AddListingController, MakeListingSecret, Failed to make listing secret");
                    }

                    TempData["SuccessMessage"] = "آگهی با موفقیت ویرایش شد";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error Updating listing ");
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


		[Authorize(Policy = "EditListingPolicy")]
		[HttpPost]
        public async Task<IActionResult> DeleteListingImage(string imagePath)
        {
            try
            {
                await _service.DeleteImageFromFTP(imagePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddListingController, DeleteListingAsync, Failed to delete listing");
                ModelState.AddModelError(ex.Message, "حذف عکس آگهی موفقیت آمیز نبود");
            }
            finally {
                TempData["SuccessMessage"] = "عکس آگهی با موفقیت حذف شد";
            }

			return RedirectToAction("Index", "Home");
        }
    }
}
