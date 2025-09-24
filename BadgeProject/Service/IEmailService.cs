using BadgeProject.DTO;

namespace BadgeProject.Service
{
    public interface IEmailService
    {
        Task<AuthResponse> SendOtpAsync(EmailDTO emailDto);
    }
}
