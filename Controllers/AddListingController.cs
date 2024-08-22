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
        private readonly IWebHostEnvironment _webHostEnvironment; //برای دسترسی به محیط هاستینگ و پوشه ها برای ذخیره سازی عکس لازمه

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

        
        public async Task<IActionResult> Create(Listing listing, IFormFile? ImageFile) //I want to create a service that calls the form and handles the validation
        {
            //from here
            if (ImageFile == null)
            {
                listing.ImagePath = "/images/" + "No_Image_Available.jpg";
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                listing.ImagePath = "/images/" + fileName;
            }

            //to propbably around here is fine Id like to include the model validation and date time setting but the  _context.Listings.Add(listing); is specific and not part of service
            if (ModelState.IsValid)
            {
                listing.DateTimeOfPosting = DateTime.Now; 

                _context.Listings.Add(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
                ViewBag.ModelStateErrors += error.ErrorMessage + "\n";
            }

            return View("Index", listing);
        }
    }
}
