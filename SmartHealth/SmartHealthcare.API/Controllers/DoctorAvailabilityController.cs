using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.DTOs;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;

namespace SmartHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorAvailabilityController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ POST: Add Availability (Doctor)
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddAvailability(DoctorAvailabilityDto dto)
        {
            var userId = _userManager.GetUserId(User);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                return NotFound("Doctor not found.");

            var availability = new DoctorAvailability
            {
                DoctorId = doctor.DoctorId,
                AvailableDate = dto.AvailableDate.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Notes = dto.Notes
            };

            _context.DoctorAvailabilities.Add(availability);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Availability added successfully." });
        }

        // ✅ GET: View My Availabilities
        [HttpGet("my")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyAvailabilities()
        {
            var userId = _userManager.GetUserId(User);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                return NotFound("Doctor not found.");

            var availabilities = await _context.DoctorAvailabilities
                .Where(a => a.DoctorId == doctor.DoctorId)
                .OrderBy(a => a.AvailableDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();

            return Ok(availabilities);
        }
    }
}
