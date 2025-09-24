using System.ComponentModel.DataAnnotations;

namespace BadgeProject.DTO
{
    public class VerifyEmailDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "OTP is required")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "OTP must be exactly 4 digits")]
        public string Otp { get; set; }= null!;
    }
}
