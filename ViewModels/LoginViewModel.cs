using System.ComponentModel.DataAnnotations;

namespace ASPNETLIVE.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "อีเมล ห้ามว่าง")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "รหัสผ่าน ห้ามว่าง")]
        [StringLength(100, ErrorMessage = "รหัสผ่านอย่างน้อย {2} ตัวอักษร ไม่เกิน {1} ตัวอักษร", MinimumLength = 3)]
        public string Password { get; set; } = null!;
    }
}
