using System.Diagnostics;
using System.Reflection;
using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivarClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DivarCloneContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IListingService _service;

        public HomeController(ILogger<HomeController> logger , DivarCloneContext context, IWebHostEnvironment webHostEnvironment, IListingService service)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _service = service;
        }

        public IActionResult Index()
        {
            var listings = _service.GetAllListings();
            return View(listings);
        }


        //public IActionResult FilterResults(string category)
        //{
        //    if (Enum.TryParse(typeof(Category), category, out var categoryEnum))
        //    {
        //        var listings = _service.FilterResult(category, categoryEnum);
        //        return View("index", listings);
        //    }
        //    return RedirectToAction("Index");
        //}

        //public IActionResult SearchResults(string textToSearch)
        //{
        //    var listings = _service.SearchResult(textToSearch);
        //    return View("index", listings);
        //}

        //[Authorize]
        //public IActionResult ShowUserListings(string Username)
        //{           
        //    var listings = _service.ShowUserListings(Username);
        //    return View("index", listings);
        //}

        [Authorize]
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
