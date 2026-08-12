using Microsoft.EntityFrameworkCore;
using Ecom.Application.DTOs;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Ecom.Application.Interfaces;

namespace Ecom.API.Controllers.AllUserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly string secretKey = "9Xv$7mK#2QpL@9Bn!4RtYw&6HsJcD3FgUa5BeVm1";

        public AuthController(IConfiguration config, AppDbContext context, IEmailService emailService)
        {
            _config = config;
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var user = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Email == login.Email);

            if (user == null)
                return Unauthorized("Invalid Email or Password");

            bool isPasswordValid = false;
            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            }
            catch
            {
                isPasswordValid = user.Password == login.Password;
            }

            if (!isPasswordValid)
                return Unauthorized("Invalid Email or Password");

            var role = user.UserRoles
                           .Select(ur => ur.Role.RoleName)
                           .FirstOrDefault() ?? "User";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role)
                }),

                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                Name = user.Name,
                Email = user.Email,
                Role = role
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found with this email.");
            }

            var token = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var resetUrl = $"http://localhost:5173/reset-password?token={token}&email={Uri.EscapeDataString(user.Email)}";
            var htmlContent = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);'>
                    <div style='background-color: #4F46E5; padding: 24px; text-align: center; color: white;'>
                        <h1 style='margin: 0; font-size: 24px;'>Password Reset Request</h1>
                    </div>
                    <div style='padding: 24px; color: #333333; line-height: 1.6;'>
                        <p>Hello {user.Name},</p>
                        <p>We received a request to reset the password for your e-commerce account. If you did not request this, you can safely ignore this email.</p>
                        <div style='text-align: center; margin: 32px 0;'>
                            <a href='{resetUrl}' style='background-color: #4F46E5; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: bold; display: inline-block;'>Reset Your Password</a>
                        </div>
                        <p>This link is valid for 1 hour.</p>
                        <hr style='border: none; border-top: 1px solid #e0e0e0; margin: 24px 0;' />
                        <p style='font-size: 12px; color: #666666;'>If you are having trouble clicking the button, copy and paste this URL into your web browser:<br/><a href='{resetUrl}'>{resetUrl}</a></p>
                    </div>
                </div>";

            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", htmlContent);

            return Ok(new { message = "Reset link has been sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired reset token.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password has been reset successfully." });
        }
    }

    public class ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
