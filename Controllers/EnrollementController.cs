using Microsoft.AspNetCore.Mvc;
using DivarClone.Models;
using DivarClone.Services;
using DivarClone.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


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
        public IActionResult RegisterationPage()
        {
            return View("index");
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

        public string status;

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View("login");
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(Enroll e)
        {
            var principal = await _service.LogUserIn(e);

            if (principal != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Home/Index");
            }
            else
            {
                ViewData["Message"] = "User Login Details Failed!!";
                return View("login");
            }
        }


        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    Session.Abandon();
        //    return RedirectToAction("Index", "UserLogin");
        //}

    }
}
