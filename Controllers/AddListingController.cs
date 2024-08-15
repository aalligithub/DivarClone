using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//کنترلر مشابه سیستم views.py python
namespace DivarClone.Controllers
{
    
    [Authorize] //[Authorize] برای اجباری کردن لاگین صفحه
    public class AddListingController : Controller
    {
        //اضافه کردن دو قابلیت یکی لاگر برای ثبت وضعیت اضافه و خواندن دیتا بیس و دیگری کانتکست مدل و دیتابیس
        private readonly ILogger<AddListingController> _logger;
        private readonly DivarCloneContext _context;

        public AddListingController(DivarCloneContext context, ILogger<AddListingController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //صفحه با گت ریکوست یا همان درخواست url نمایش داده میشود
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] //درخواست post یا ارسال محتوا به دیتابیس با این اکشن هندل میشود
        [ValidateAntiForgeryToken] //جلوگیری از حمله CSRF

        
        public async Task<IActionResult> Create(Listing listing) //اکشن create برای ایجاد لیستینگ جدید که از listing ارث بری میکند
        {
            if (ModelState.IsValid) 
            {
                listing.DateTimeOfPosting = DateTime.Now; //چون تاریخ لیستینگ یا اگهی همان زمان انتشار است همینجا با سیستم اضافه میکنیم به شرط انکه خطایکاربر نداشته باشیم

                _context.Listings.Add(listing); //اضافه کردن لیستینگ به دیتا بیس به صورت Async که مطمعن شویم انجام میشود و interupt نمیشود
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Listing"); // بازگردانی به صفحه اصلی بعد از انتشار
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors); //درصورت وجود خطای کاربر بدین صورت نمایش داده میشود خطاها در مدل سازی تایین شده اند
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }

            return View("Index", listing); //اگر اروری وجود داشته باشد با این دستور هم ارور ها هم صفحه را برای ویرایش نشان میدهیم
        }
    }
}
