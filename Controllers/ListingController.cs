using Microsoft.AspNetCore.Mvc;

namespace DivarClone.Controllers
{
    public class ListingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
