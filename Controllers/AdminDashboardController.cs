using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;


namespace DivarClone.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAdminService _service;

        public AdminDashboardController(ILogger<AdminDashboardController> logger, IWebHostEnvironment webHostEnvironment, IAdminService service)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _service = service;
        }

        [Authorize(Policy = "AdminOrPermittedDashView")]
        //[Authorize(Policy = "ViewDashboardPolicy")]
		public IActionResult Index()
        {
            var Users = _service.GetAllUsers();
            return View(Users);
        }

        //public async Task<IActionResult> ChangeUserRole(int Id)
        //{
        //    await _service.ChangeUserRoles(Id);

        //    return RedirectToAction("Index");
        //}

        //public async Task<IActionResult> GiveUserSpecialPermission(int Id)
        //{
        //    await _service.GiveUserSpecialPermission(Id);

        //    return RedirectToAction("Index");
        //}
    }
}
