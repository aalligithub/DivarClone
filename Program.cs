using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DivarClone.Areas.Identity.Data;
using DivarClone.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DivarCloneContextConnection") ?? throw new InvalidOperationException("Connection string 'DivarCloneContextConnection' not found.");

builder.Services.AddDbContext<DivarCloneContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<DivarCloneUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DivarCloneContext>();

builder.Services.AddScoped<IListingService, ListingService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
