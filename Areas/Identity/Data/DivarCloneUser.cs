using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DivarClone.Areas.Identity.Data;

//ارث بری و اضافه کردن دو پراپرتی جدید به مدل یوزر دیفالت
public class DivarCloneUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

