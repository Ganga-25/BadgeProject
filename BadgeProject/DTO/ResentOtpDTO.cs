using System.ComponentModel.DataAnnotations;

namespace BadgeProject.DTO
{
    public class ResentOtpDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; } = null!;
    }
}
