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
    [Authorize(Roles = "Admin")]
    public class DoctorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public DoctorsController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ✅ Add new doctor (user + profile)
        [HttpPost("add")]
        public async Task<IActionResult> AddDoctor(DoctorDto model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                Role = "Doctor"
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                UserId = user.Id,
                Specialization = model.Specialization,
                Qualification = model.Qualification,
                Experience = model.Experience,
                Availability = model.Availability
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Doctor added successfully" });
        }

        // ✅ Update profile of an existing doctor
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorDto model)
        {
            var doctor = await _context.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return NotFound("Doctor not found");

            // Only allow the doctor themselves or admin to update
            var isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!isAdmin && doctor.UserId != userId)
                return Forbid("You are not authorized to update this profile.");

            doctor.Specialization = model.Specialization;
            doctor.Qualification = model.Qualification;
            doctor.Experience = model.Experience;
            doctor.Availability = model.Availability;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Doctor profile updated successfully" });
        }

        // ✅ Get all doctors
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    d.DoctorId,
                    d.Specialization,
                    d.Qualification,
                    d.Experience,
                    d.Availability,
                    FullName = d.User.FullName,
                    Email = d.User.Email
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // ✅ Filter/search by specialization (public access)
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBySpecialization([FromQuery] string specialization)
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.Specialization.ToLower().Contains(specialization.ToLower()))
                .Select(d => new
                {
                    d.DoctorId,
                    d.Specialization,
                    d.Qualification,
                    d.Experience,
                    d.Availability,
                    FullName = d.User.FullName,
                    Email = d.User.Email
                })
                .ToListAsync();

            return Ok(doctors);
        }
    }
}
