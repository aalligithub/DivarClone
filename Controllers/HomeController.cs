using System.Diagnostics;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DivarClone.Attributes;


namespace DivarClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IListingService _service;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IListingService service)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _service = service;
        }


        public IActionResult Index()
        {
            var listings = _service.GetAllListings();
            return View(listings);
        }


        [Authorize(Policy = "ViewSpecialListingPolicy")]
        [HttpGet("/SecretListings")]
        public IActionResult SecretListings() {

            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var secretListings = new List<Listing>();

            try {secretListings = _service.GetSecretListings(UserId); }
            catch (Exception ex) {
                _logger.LogError(ex," HomeController, GetSecretListings, No listings were found");
                secretListings.Clear();
             }

            if (secretListings == null)
            {
                var listings = _service.GetAllListings();
                ViewBag.ModelStateErrors += "سطح دسترسی لازم برای آگهی را ندارید";
                return PartialView("_ListingPartial", listings);
            }
            
			else if (secretListings.Count() > 0)
			{
                return PartialView("_ListingPartial", secretListings);
            }
            else
            {
                ViewBag.ModelStateErrors += "آگهی خاص یافت نشد";

                var listings = _service.GetAllListings();
                return PartialView("_ListingPartial", listings);
            }
            
        }


        [HttpGet("/Listings/Details/{id}")]
        public IActionResult Details(int id)
        {
            var listing = _service.GetSpecificListing(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }


		public IActionResult FilterResults(string category)
        {
            var filteredListings = new List<Listing>();

            if (Enum.TryParse(typeof(Category), category, out var categoryEnum))
            {
                try
                {
                    filteredListings = _service.FilterResult(categoryEnum);
                    if (filteredListings.Count > 0)
                    {
                        return PartialView("_ListingPartial", filteredListings);
                    }
                    else {
                        var listings = _service.GetAllListings;
                        ViewBag.ModelStateErrors += "آگهی برای فیلتر یافت نشد";
                        return PartialView("_ListingPartial", listings);
                    }
                }
                catch (Exception ex) {
                    _logger.LogError(ex ,"HomeController, FilterResults, couldnt get matching listings");
                }
            }
            ViewBag.ModelStateErrors += "فیلتر تعریف نشده است";
            return RedirectToAction("Index");
        }


        public IActionResult SearchResults(string textToSearch)
        {
            if (textToSearch != null)
            {
                var listings = new List<Listing>();

                try { 
                    listings = _service.SearchResult(textToSearch);
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "HomeController, SearchResults, Couldnt get searched listings");
                    return RedirectToAction("Index");
                };
                
                if ( listings.Count > 0) { 
                    return PartialView("_ListingPartial", listings);
                } 
                else
                {
                    return NotFound();
                }
            }
            else
            { 
                ViewBag.ModelStateErrors += "برای جست و جو اسم آگهی مورد نظر را وارد کنید ";
                var listings = new List<Listing>();

                try
                {
                   listings = _service.GetAllListings();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "HomeController, SearchResults, Couldnt get searched listings");
                    return RedirectToAction("Index");
                };

                return PartialView("_ListingPartial", listings);
            }
        }


        [Authorize]
        public IActionResult ShowUserListings(string Username)
        {
            TempData["SuccessMessage"] = $"آگهی های کاربر : {User.Identity.Name}";

            var listings = _service.ShowUserListings(Username);
            return PartialView("_ListingPartial", listings);
        }


		[Authorize(Policy = "DeleteListingsPolicy")]
		public async Task<IActionResult> DeleteUserListing(int id)
        {
            try
            {
                await _service.DeleteUserListing(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddListingController, DeleteListingAsync, Failed to delete listing");
                ModelState.AddModelError(ex.Message, "حذف آگهی موفقیت آمیز نبود");
            }
            finally
            {
                TempData["SuccessMessage"] = "آگهی با موفقیت حذف شد";
            }
            
            var listings = _service.GetAllListings();
            return PartialView("_ListingPartial", listings);
            //return View("Index", listings);
        }


        [Authorize]
        public IActionResult UserControlPartial() {
            return PartialView("_UserControl");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
