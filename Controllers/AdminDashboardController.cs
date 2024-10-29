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
        public async Task<IActionResult> SearchUsers(string Username)
        {
            var Users = _service.SearchUsers(Username);
            var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
            var AllPossibleRoles = await _service.GetAllPossibleRoles();

            ViewBag.Users = Users;
            ViewBag.AllPossiblePermissions = AllPossiblePermissions;
            ViewBag.AllPossibleRoles = AllPossibleRoles;

            return PartialView("_AdminUsersPartial");
        }

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int Id, int Role)
        {
			TempData["UserId"] = Id.ToString();

			try
            {
                await _service.ChangeUserRoles(Id, Role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AdminController, ChangeUserRole, Failed to change user's role");

				ViewBag.ModelStateErrors += "تغییر نقش کاربر موفقیت آمیز نبود";
			}
            finally {
                TempData["SuccessMessage"] = "نقش کاربر تغییر کرد";
            }

            var Users = _service.GetAllUsers();
            var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
            var AllPossibleRoles = await _service.GetAllPossibleRoles();

            ViewBag.Users = Users;
            ViewBag.AllPossiblePermissions = AllPossiblePermissions;
            ViewBag.AllPossibleRoles = AllPossibleRoles;

            return PartialView("_AdminUsersPartial");
        }

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        [HttpPost]
        public async Task<IActionResult> GiveUserSpecialPermission(int Id, int PermissionId)
        {
			TempData["UserId"] = Id.ToString();

			try
            {
                await _service.GiveUserSpecialPermission(Id, PermissionId);
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "AdminController, ChangeUserRole, Failed to give user special permission");

				ViewBag.ModelStateErrors += "اضافه کردن اجازه خاص به کاربر موفقیت آمیز نبود";
			}
			finally
			{
				TempData["SuccessMessage"] = "اجازه خاص اضافه شد";
			}

			var Users = _service.GetAllUsers();
			var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
			var AllPossibleRoles = await _service.GetAllPossibleRoles();

			ViewBag.Users = Users;
			ViewBag.AllPossiblePermissions = AllPossiblePermissions;
			ViewBag.AllPossibleRoles = AllPossibleRoles;

			return PartialView("_AdminUsersPartial");
		}

        [RoleOrPermissionAuthorize(Role = "Admin", Permission = "CanViewDashboard")]
        public async Task<IActionResult> RemoveUserSpecialPermission(int Id, string PermissionName) {
			TempData["UserId"] = Id.ToString();

			try
			{
				await _service.RemoveUserSpecialPermission(Id, PermissionName);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "AdminController, RemoveUserSpecialPermission, Failed to remove user special permission");

				ViewBag.ModelStateErrors += "حذف کردن اجازه خاص از کاربر موفقیت آمیز نبود";
			}
			finally
			{
				TempData["SuccessMessage"] = "اجازه خاص حذف شد";
			}

			var Users = _service.GetAllUsers();
			var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
			var AllPossibleRoles = await _service.GetAllPossibleRoles();

			ViewBag.Users = Users;
			ViewBag.AllPossiblePermissions = AllPossiblePermissions;
			ViewBag.AllPossibleRoles = AllPossibleRoles;

			return PartialView("_AdminUsersPartial");
		}
    }
}
