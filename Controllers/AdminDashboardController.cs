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

        [Authorize(Policy = "AdminOrPermittedDashView")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int Id, int Role)
        {
            await _service.ChangeUserRoles(Id, Role);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "AdminOrPermittedDashView")]
        [HttpPost]
        public async Task<IActionResult> GiveUserSpecialPermission(int Id, int PermissionId)
        {
            await _service.GiveUserSpecialPermission(Id, PermissionId);

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> RemoveUserSpecialPermission(int Id, int PermissionId) { }
    }
}
