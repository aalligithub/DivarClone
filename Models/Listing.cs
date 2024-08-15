using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace DivarClone.Models //مدل اگهی های ما که محتوایی که ما برای اگهی کردن لازم داریم را پوشش میدهد
{    
    public enum Category //enum تعریف کردیم که کتگوری فقط از اعضای از پیش تایین شده انتخاب شود
    {
        Electronics,
        Realstate,
        Vehicles
    }

    public class Listing    //اسم مدل ما listing است که به کلاس های دیگه داخل کانتکست و کنترلر پاس شده
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public int Price { get; set; }

        public required string Poster { get; set; }

        public Category Category { get; set; }

        public DateTime DateTimeOfPosting { get; set; }
    }
}
