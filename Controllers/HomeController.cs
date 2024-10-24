﻿using System.Diagnostics;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

        //[Authorize(Role = "Admin" || Permission = "CanViewDashBoard")]
        [HttpGet("/SecretListings")]
        public IActionResult SecretListings() {

            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var listing = _service.GetSecretListings(UserId);
			if (listing == null)
			{
                ViewBag.ModelStateErrors += "سطح دسترسی مورد نیاز را ندارید";

                var listings = _service.GetAllListings();
                return View("Index", listings);
			}

			return View("Index", listing);
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
                ViewBag.ModelStateErrors += "برای جست و جو اسم آگهی مورد نظر را وارد کنید ";
                var listings = _service.GetAllListings();
                return View("Index", listings);
            }
        }

        [Authorize]
        public IActionResult ShowUserListings(string Username)
        {
            TempData["SuccessMessage"] = $"آگهی های کاربر : {User.Identity.Name}";

            var listings = _service.ShowUserListings(Username);
            return View("index", listings);
        }

        [Authorize(Roles = "Admin, User")]
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
