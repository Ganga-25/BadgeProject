using System.ComponentModel.DataAnnotations;

namespace BadgeProject.DTO
{
    public class RegisterDTO
    {
       

        [Required(ErrorMessage = "UserName is required")]
        [RegularExpression(@"^[A-Za-z].*", ErrorMessage = "Name must start with a letter.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain an uppercase letter, a number, and a special character.")]

        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]

        public string PhoneNumber { get; set; }= null!;

    }
}
