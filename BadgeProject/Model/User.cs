namespace BadgeProject.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; }=null!;

        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserOtp> userOtps { get; set; }



    }
}
