using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Services;
using DivarClone.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DivarClone.Controllers
{
    public class EnrollementController : Controller
    {
        private readonly ILogger<EnrollementController> _logger;
		private readonly IEnrollService _service;

		public EnrollementController(ILogger<EnrollementController> logger, IEnrollService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Enroll e)
        {
            Enroll er = new Enroll();
            bool result = await _service.EnrollUser(e);
            if (result)
            {
                //Redirect to login
                return View();
            }
            else {
                //display register form and errors
                return View();
            }

        }
    }
}
