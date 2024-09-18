using System.ComponentModel.DataAnnotations;

namespace DivarClone.Models //مدل اگهی های ما که محتوایی که ما برای اگهی کردن لازم داریم را پوشش میدهد
{
    public enum Category //enum تعریف کردیم که کتگوری فقط از اعضای از پیش تایین شده انتخاب شود
    {
        [Display(Name="وسایل برقی")]
        Electronics,

        [Display(Name = "املاک")]
        Realstate,

        [Display(Name = "وسایل نقلیه")]
        Vehicles
    }

    public class Listing    //اسم مدل ما listing است که به کلاس های دیگه داخل کانتکست و کنترلر پاس شده
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام آگهی را وارد کنید")]
        public string Name { get; set; }

        [Required(ErrorMessage = "توضیحات آگهی را وارد کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "قیمت کالا را وارد کنید")]
        public int? Price { get; set; }

        public string Poster { get; set; }

        [Required(ErrorMessage = "برای آگهی خود دسته بندی انتخاب کنید")]
        public Category? Category { get; set; }

        public DateTime DateTimeOfPosting { get; set; }

        public string? ImagePath { get; set; }  //این فیلد محل ذخیره سازی عکس هارو سیو میکنه که برای نشون دادن نیازشون داریم چون عکس الزامی نیست nullable
    }
}
