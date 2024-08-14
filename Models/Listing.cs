using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace DivarClone.Models
{
    //enum تعریف کردیم که کتگوری فقط از اعضای از پیش تایین شده انتخاب شود
    public enum Category
    {
        Electronics,
        Realstate,
        Vehicles
    }

    public class Listing
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
