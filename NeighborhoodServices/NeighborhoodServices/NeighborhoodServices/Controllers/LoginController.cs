using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NeighborhoodServices.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NeighborhoodServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            // Very basic user validation for now
            if (login.Email == "jane@example.com" && login.Password == "test123")
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "2"),
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Name, "Jane Customer"),
                    new Claim(ClaimTypes.Role, "Customer"),
                    new Claim(ClaimTypes.Email, "jane@example.com")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
