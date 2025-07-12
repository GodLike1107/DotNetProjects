using BCrypt.Net;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NeighborhoodServices.API.Data;
using NeighborhoodServices.API.Models;
using NeighborhoodServices.API.Services;
using NeighborhoodServices.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NeighborhoodServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly NeighborhoodDbContext _context;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;
    private readonly IJwtService _jwtService;

    public AuthController(
        NeighborhoodDbContext context,
        IConfiguration config,
        IEmailSender emailSender,
        IJwtService jwtService)
    {
        _context = context;
        _config = config;
        _emailSender = emailSender;
        _jwtService = jwtService;
    }

    // 🔐 Login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }

    // 📝 Signup
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest("User already exists with this email.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = hashedPassword,
            Role = request.Role
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok("Signup successful.");
    }

    // 🔁 Forgot Password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user == null)
            return NotFound("User with this email does not exist.");

        var resetLink = $"{request.ClientAppUrl.TrimEnd('/')}/reset-password?email={Uri.EscapeDataString(user.Email)}";

        var subject = "Password Reset - Neighborhood Services";
        var body = $@"
            <p>Hello {user.Name},</p>
            <p>You requested to reset your password. Click the link below to reset it:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>This link will expire in 24 hours.</p>";

        try
        {
            await _emailSender.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to send email: {ex.Message}");
            return StatusCode(500, "Failed to send reset email.");
        }

        return Ok(new { message = "If an account exists for this email, a reset link has been sent." });
    }

    // 🔒 Reset Password
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (user == null)
            return NotFound("User not found");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _context.SaveChangesAsync();

        var subject = "Password Reset Confirmation";
        var body = $@"
            <p>Hello {user.Name},</p>
            <p>Your password has been reset successfully.</p>
            <p>If you did not perform this action, please contact support immediately.</p>";

        await _emailSender.SendEmailAsync(user.Email, subject, body);

        return Ok("Password has been reset successfully.");
    }

    // 🌐 Google Login
    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

            // Optional: Check if the user already exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                // Create new user
                user = new User
                {
                    Name = payload.Name,
                    Email = payload.Email,
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Generate JWT token for the user
            var token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }
        catch (InvalidJwtException)
        {
            return Unauthorized("Invalid Google ID token.");
        }
    }

    // 📧 Email Test
    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        await _emailSender.SendEmailAsync("ayush11079@gmail.com", "Test Email", "<p>This is a test email from API</p>");
        return Ok("Sent");
    }
}
