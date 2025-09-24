using BadgeProject.DTO;
using BadgeProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadgeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // 1. Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.RegisterAsync(registerDto);
            return StatusCode(response.StatusCode, response);
        }

        // 2. Verify OTP
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyEmailDTO verifyOtpDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.VerifyOTP(verifyOtpDto);
            return StatusCode(response.StatusCode, response);
        }

        // 3. Resend OTP
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromForm] ResentOtpDTO resendOtpDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.RegenerateOtp(resendOtpDto);
            return StatusCode(response.StatusCode, response);
        }

        // 4. Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.LoginAsync(loginDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}

