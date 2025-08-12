using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHealthcare.Core.DTOs;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class AvailabilityController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AvailabilityController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAvailability(AvailabilityDto dto)
    {
        var userId = _userManager.GetUserId(User);
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
            return NotFound("Doctor not found");

        var availability = new Availability
        {
            DoctorId = doctor.DoctorId,
            AvailableFrom = dto.AvailableFrom,
            AvailableTo = dto.AvailableTo
        };

        _context.Availabilities.Add(availability);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Availability added successfully" });
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyAvailability()
    {
        var userId = _userManager.GetUserId(User);
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
            return NotFound("Doctor not found");

        var availabilities = await _context.Availabilities
            .Where(a => a.DoctorId == doctor.DoctorId)
            .ToListAsync();

        return Ok(availabilities);
    }
}
