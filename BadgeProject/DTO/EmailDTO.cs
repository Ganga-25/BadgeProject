using System.ComponentModel.DataAnnotations;

namespace BadgeProject.DTO
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;

        public string? Subject { get; set; } = "Your OTP Code";
        public string? Body { get; set; }
    }
}
