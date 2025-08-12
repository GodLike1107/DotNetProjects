using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("✅ This is a public endpoint. No auth required.");
        }

        [Authorize] // Requires JWT token
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("🔒 This is a secure endpoint. You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminEndpoint()
        {
            return Ok("👑 Only Admins can access this endpoint!");
        }
    }
}
