using BadgeProject.DTO;

namespace BadgeProject.Service
{
    public interface IAuthService
    {
       Task<AuthResponse> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthResponse> VerifyOTP (VerifyEmailDTO verifyEmailDTO);
        Task<AuthResponse> LoginAsync (LoginDTO loginDTO);
        Task<AuthResponse> RegenerateOtp(ResentOtpDTO resentOtpDTO);


    }
}
