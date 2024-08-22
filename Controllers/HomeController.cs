using System.Diagnostics;
using System.Reflection;
using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
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

        public HomeController(ILogger<HomeController> logger , DivarCloneContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var listings = _context.Listings.ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return View(listings);
        }

        public IActionResult FilterResults(string category)
        {
            if (Enum.TryParse(typeof(Category), category, out var categoryEnum))
            {
                var listings = _context.Listings.Where(l => l.Category == (Category)categoryEnum).ToList();

                foreach (var listing in listings)
                {
                    if (string.IsNullOrEmpty(listing.ImagePath) ||
                        !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                    {
                        listing.ImagePath = "/images/No_Image_Available.jpg";
                    }
                }
                return View("index", listings);
            }
            return RedirectToAction("Index");
        }

        public IActionResult SearchResults(string textToSearch)
        {
            var listings = _context.Listings.Where(l => l.Name.Contains(textToSearch)).ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return View("index", listings);
        }

        public IActionResult ShowUserListings(string Username)
        {
            var listings = _context.Listings.Where(l => l.Poster == Username).ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return View("index", listings);
        }

        public IActionResult DeleteUserListing(int id)
        {
            var listingToDelete = _context.Listings.FirstOrDefault(l => l.Id == id);

            if (listingToDelete == null)
            {
                return RedirectToAction("Index");
            }
            else { 
                _context.Listings.Remove(listingToDelete);
                _context.SaveChanges();

                var listings = _context.Listings.ToList();

                foreach (var listing in listings)
                {
                    if (string.IsNullOrEmpty(listing.ImagePath) ||
                        !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                    {
                        listing.ImagePath = "/images/No_Image_Available.jpg";
                    }
                }

                return View("index", listings);
            }
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
