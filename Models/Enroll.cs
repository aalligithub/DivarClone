using System.ComponentModel.DataAnnotations;

namespace DivarClone.Models
{
    public class Enroll
    {
        [Display(Name = "ID")]
        public int ID { get; set; }


        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }


        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "رمز عبور \"{0}\" میبایست {2} کارکتر باشد", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "رمز عبور میباست شامل: حداقل هشت کارکتر, حداقل یک حرف بزرگ,  حداقل یک حرف کوچک, 1 عدد, و یک کارکتر خاص باشد")]
        public string Password { get; set; }

        [Display(Name = "تایید رمز عبور")]
        [Required(ErrorMessage = "لطفا برای تایید رمز خود را مجدد وارد کنید")]
        [Compare("Password", ErrorMessage = "رمز عبور تطابق ندارد لطفا مجدد تلاش کنید")]
        [DataType(DataType.Password)]
        public string Confirmpwd { get; set; }

        public Nullable<bool> Is_Deleted { get; set; }


        [Required(ErrorMessage = "ایمیل الزامیست")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "ایمیل وارد شده درست نمیباشد")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس الزامیست")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره همراه وارد شده درست نمیباشد")]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public List<Enroll> Enrollsinfo { get; set; }
    }
}
