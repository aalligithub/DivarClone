using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DivarClone.Areas.Identity.Data;

//سیستم orm و مپینگ دیتابیس مشابه django models
public class DivarCloneContext : IdentityDbContext<DivarCloneUser>
{
    public DivarCloneContext(DbContextOptions<DivarCloneContext> options)
        : base(options)
    {
    }

    //اضافه کردن قالب اگهی ها listing model به کانتکست
    public DbSet<Listing> Listings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

//کلاس برای سیستم Entity و User
public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<DivarCloneUser>
{
    public void Configure(EntityTypeBuilder<DivarCloneUser> builder)
    {
        //خصوصیت های جدید برای عضویت کاربران
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
    }
}