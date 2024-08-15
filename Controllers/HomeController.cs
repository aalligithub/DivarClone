using System.Diagnostics;
using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Mvc;

namespace DivarClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DivarCloneContext _context;

        public HomeController(ILogger<HomeController> logger , DivarCloneContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var listings = _context.Listings.ToList();
            return View(listings);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
