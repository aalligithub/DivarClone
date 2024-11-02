using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Services;
using Microsoft.AspNetCore.Authorization;
using DivarClone.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace DivarClone.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAdminService _service;
        private readonly IEnrollService _enrollService;

        public AdminDashboardController(ILogger<AdminDashboardController> logger, IWebHostEnvironment webHostEnvironment, IAdminService service, IEnrollService enrollService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _service = service;
            _enrollService = enrollService;
        }

		[Authorize(Policy = "ViewDashboardPolicy")]
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

		[Authorize(Policy = "ViewDashboardPolicy")]
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

		[Authorize(Policy = "ViewDashboardPolicy")]
		[HttpPost]
        public async Task<IActionResult> ChangeUserRole(int Id, int Role, string Username)
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

            TempData["SuccessMessage"] = "نقش کاربر تغییر کرد";

            //user changed their own permission and needs to login again
            if (User.Identity.Name == Username)
            {
                TempData["SuccessMessage"] = "برای دریافت اجازه ها و نقش جدید مجددا لاگین کنید";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Json(new { redirectUrl = Url.Action("Index", "Home") });
            }

            var Users = _service.GetAllUsers();
            var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
            var AllPossibleRoles = await _service.GetAllPossibleRoles();

            ViewBag.Users = Users;
            ViewBag.AllPossiblePermissions = AllPossiblePermissions;
            ViewBag.AllPossibleRoles = AllPossibleRoles;

            return PartialView("_AdminUsersPartial");
        }

        [Authorize(Policy = "ViewDashboardPolicy")]
        [HttpPost]
        public async Task<IActionResult> GiveUserSpecialPermission(int Id, int PermissionId, string Username)
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

            //user changed their own permission and needs to login again
            if (User.Identity.Name == Username)
            {
                TempData["SuccessMessage"] = "برای دریافت اجازه ها و نقش جدید مجددا لاگین کنید";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Json(new { redirectUrl = Url.Action("Index", "Home") });
            }

            var Users = _service.GetAllUsers();
			var AllPossiblePermissions = await _service.GetAllPossiblePermissions();
			var AllPossibleRoles = await _service.GetAllPossibleRoles();

			ViewBag.Users = Users;
			ViewBag.AllPossiblePermissions = AllPossiblePermissions;
			ViewBag.AllPossibleRoles = AllPossibleRoles;

			return PartialView("_AdminUsersPartial");
		}

        [Authorize(Policy = "ViewDashboardPolicy")]
        public async Task<IActionResult> RemoveUserSpecialPermission(int Id, string PermissionName, string Username) {
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

            //user changed their own permission and needs to login again
            if (User.Identity.Name == Username)
            {
                TempData["SuccessMessage"] = "برای دریافت اجازه ها و نقش جدید مجددا لاگین کنید";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Json(new { redirectUrl = Url.Action("Index", "Home") });
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
