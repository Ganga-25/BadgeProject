using BadgeProject.DTO;
using System.Net;
using System.Net.Mail;

namespace BadgeProject.Service
{
    public class EmailService:IEmailService
    {
        private readonly string _fromEmail = "gangasureshv21@gmail.com";
        private readonly string _fromName = "Badge App";
        private readonly string _password = "thbnvzylqamskzmf"; 
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        public async Task<AuthResponse> SendOtpAsync(EmailDTO emailDto)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(emailDto.ToEmail);
                message.Subject = emailDto.Subject ?? "Your OTP Code";
                message.Body = emailDto.Body ?? $"Your OTP code is {emailDto.Otp}. It is valid for 2 minutes.";
                message.IsBodyHtml = true;

                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_fromEmail, _password),
                    EnableSsl = true
                };

                await client.SendMailAsync(message);

                return new AuthResponse(200, "OTP Sent to email successfully.");
            }
            catch (Exception ex)
            {
                return new AuthResponse (500, $"SMTP error: {ex.Message}" );
            }

        }
    }
}
