using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using DivarClone.Attributes;


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

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        public async Task<IActionResult> IndexAsync()
        {
			var Users = _service.GetAllUsers();
            var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
            var AllPossibleRoles = await _service.GetAllPossibleRoles();

            ViewBag.Users = Users;
            ViewBag.AllPossiblePermissions = AllPossiblePermissions;
            ViewBag.AllPossibleRoles = AllPossibleRoles;

            return View();
        }

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int Id, int Role)
        {
            await _service.ChangeUserRoles(Id, Role);

            return RedirectToAction("Index");
        }

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        [HttpPost]
        public async Task<IActionResult> GiveUserSpecialPermission(int Id, int PermissionId)
        {
            await _service.GiveUserSpecialPermission(Id, PermissionId);

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> RemoveUserSpecialPermission(int Id, int PermissionId) { }
    }
}
