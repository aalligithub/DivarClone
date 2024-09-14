using System.Diagnostics;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;


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


        public IActionResult FilterResults(string category)
        {
            if (Enum.TryParse(typeof(Category), category, out var categoryEnum))
            {
                var listings = _service.FilterResult(categoryEnum);
                return View("index", listings);
            }
            return RedirectToAction("Index");
        }

        public IActionResult SearchResults(string textToSearch)
        {
            if (textToSearch != null)
            {
                var listings = _service.SearchResult(textToSearch);
                return View("index", listings);
            }
            else {
                ModelState.AddModelError("", "برای جست و جو ");
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public IActionResult ShowUserListings(string Username)
        {
            var listings = _service.ShowUserListings(Username);
            return View("index", listings);
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteUserListing(int id)
        {
            await _service.DeleteUserListing(id);
            var listings = _service.GetAllListings();
            return View("Index", listings);
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
