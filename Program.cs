using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DivarClone.Areas.Identity.Data;
using DivarClone.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DivarCloneContextConnection") ?? throw new InvalidOperationException("Connection string 'DivarCloneContextConnection' not found.");

builder.Services.AddDbContext<DivarCloneContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<DivarCloneUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DivarCloneContext>();

builder.Services.AddScoped<IListingService, ListingService>();

builder.Services.AddScoped<IEnrollService, EnrollService>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure authentication to use cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Enrollment/login"; // Redirect to this path if user is not authenticated
        options.LogoutPath = "/Enrollment/Logout"; // Path for logging out
        options.Cookie.Name = "UserLoginCookie"; // Name of the authentication cookie
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
