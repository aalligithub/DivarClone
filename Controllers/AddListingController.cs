using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddListingController(DivarCloneContext context, ILogger<AddListingController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //صفحه با گت ریکوست یا همان درخواست url نمایش داده میشود
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] //درخواست post یا ارسال محتوا به دیتابیس با این اکشن هندل میشود
        [ValidateAntiForgeryToken] //جلوگیری از حمله CSRF

        
        public async Task<IActionResult> Create(Listing listing, IFormFile? ImageFile) //اکشن create برای ایجاد لیستینگ جدید که از listing ارث بری میکند
        {
            if (ImageFile == null)
            {
                listing.ImagePath = "/images/" + "No_Image_Available.jpg";
            }

            else if (ImageFile.Length > 0)
            {
                // Create the directory if it doesn't exist
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadDir);

                // Create a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                // Save the image to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Store the image path in the database
                listing.ImagePath = "/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                listing.DateTimeOfPosting = DateTime.Now; //چون تاریخ لیستینگ یا اگهی همان زمان انتشار است همینجا با سیستم اضافه میکنیم به شرط انکه خطایکاربر نداشته باشیم

                _context.Listings.Add(listing); //اضافه کردن لیستینگ به دیتا بیس به صورت Async که مطمعن شویم انجام میشود و interupt نمیشود
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); // بازگردانی به صفحه اصلی بعد از انتشار
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
