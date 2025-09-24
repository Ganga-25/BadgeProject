using AutoMapper;
using BadgeProject.Data;
using BadgeProject.DTO;
using BadgeProject.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BadgeProject.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public AuthService(AppDbContext context, IEmailService emailService, IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return new AuthResponse(400, "User data cannot be null");

            // Trim inputs
            registerDTO.Name = registerDTO.Name.Trim();
            registerDTO.Email = registerDTO.Email.Trim();
            registerDTO.PhoneNumber = registerDTO.PhoneNumber.Trim();
            registerDTO.Password = registerDTO.Password.Trim();

            // Check if user already exists
            var existingUser = await _context.Users
                .Include(u => u.userOtps) // Include OTPs for safety
                .SingleOrDefaultAsync(u => u.Email == registerDTO.Email);

            if (existingUser != null)
            {
                if (!existingUser.IsEmailVerified)
                {
                    // User exists but not verified → regenerate OTP
                    return await RegenerateOtp(new ResentOtpDTO { Email = existingUser.Email });
                }
                return new AuthResponse(409, "Conflict! User already exists.");
            }

            // Map DTO to User entity
            var user = _mapper.Map<User>(registerDTO);
            user.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
            user.IsEmailVerified = false;
            user.CreatedAt = DateTime.UtcNow;

            try
            {
                // Add user
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return new AuthResponse(500, "Database error while adding user: " + dbEx.InnerException?.Message ?? dbEx.Message);
            }

            // Generate OTP
            var otpCode = new Random().Next(1000, 9999).ToString("D4");
            var otp = new UserOtp
            {
                UserId = user.Id,
                OtpCode = otpCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2),
                IsUsed = false
            };

            try
            {
                await _context.UserOtps.AddAsync(otp); // Ensure DbSet is called Otps
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return new AuthResponse(500, "Database error while adding OTP: " + dbEx.InnerException?.Message ?? dbEx.Message);
            }

            // Send OTP email
            var emailDto = new EmailDTO { ToEmail = user.Email, Otp = otpCode };
            var emailResponse = await _emailService.SendOtpAsync(emailDto);

            if (emailResponse.StatusCode != 200)
                return new AuthResponse(emailResponse.StatusCode == 500 ? 500 : 400, emailResponse.Message);

            return new AuthResponse(200, "Registration successful! OTP sent to email.");
        }

        public async Task<AuthResponse> VerifyOTP(VerifyEmailDTO verifyEmailDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == verifyEmailDTO.Email);
            if (user == null) return new AuthResponse (400, "Invalid email" );

            var otp = await _context.UserOtps
                .Where(o => o.UserId == user.Id && o.OtpCode == verifyEmailDTO.Otp && !o.IsUsed)
                .OrderByDescending(o => o.ExpiresAt)
                .FirstOrDefaultAsync();

            if (otp == null) return new AuthResponse ( 400,  "Invalid OTP" );
            if (otp.ExpiresAt <= DateTime.UtcNow) return new AuthResponse (400, "OTP expired. Please regenerate.");

            otp.IsUsed = true;
            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return new AuthResponse (200,  "Email verified successfully!");
        }

        public async Task<AuthResponse> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                return new AuthResponse (400, "Invalid credentials" );

            if (!user.IsEmailVerified) return new AuthResponse ( 400, "Email not verified" );

            return new AuthResponse(200, "Login successful!");
        }

        public async Task<AuthResponse> RegenerateOtp(ResentOtpDTO resentOtpDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resentOtpDTO.Email);
            if (user == null) return new AuthResponse ( 400,  "Invalid email" );
            if (user.IsEmailVerified) return new AuthResponse ( 400,  "User already verified" );

            // Generate new OTP
            var otpCode = new Random().Next(1000, 9999).ToString("D4");
            var otp = new UserOtp
            {
                UserId = user.Id,
                OtpCode = otpCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2),
                IsUsed = false
            };

            _context.UserOtps.Add(otp);
            await _context.SaveChangesAsync();

            var emailDto = new EmailDTO { ToEmail = user.Email, Otp = otpCode };
            var emailResponse = await  _emailService.SendOtpAsync(emailDto);

            if (emailResponse.StatusCode != 200)
                return new AuthResponse (emailResponse.StatusCode,  emailResponse.Message );

            return new AuthResponse ( 200,  "New OTP sent to your email!" );
        }
    }
    }
