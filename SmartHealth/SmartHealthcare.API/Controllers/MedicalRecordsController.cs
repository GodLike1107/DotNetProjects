using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;
using SmartHealthcare.Core.DTOs;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MedicalRecordsController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ✅ Upload Medical Record
    [HttpPost("upload")]
    [Authorize(Roles = "Doctor,Patient")]
    public async Task<IActionResult> UploadRecord(MedicalRecordDto model)
    {
        var userId = _userManager.GetUserId(User);
        var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        int patientId;
        int? doctorId = null;

        if (role == "Patient")
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null) return NotFound("Patient not found");

            patientId = patient.PatientId;
        }
        else if (role == "Doctor")
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null) return NotFound("Doctor not found");

            doctorId = doctor.DoctorId;

            var patient = await _context.Patients.FindAsync(model.PatientId);
            if (patient == null) return NotFound("Patient not found");

            patientId = patient.PatientId;
        }
        else
        {
            return Forbid();
        }

        var record = new MedicalRecord
        {
            Title = model.Title,
            Description = model.Description,
            FileUrl = model.FileUrl,
            PatientId = patientId,
            DoctorId = doctorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Medical record uploaded successfully." });
    }

    // ✅ View My Records
    [HttpGet("my")]
    [Authorize(Roles = "Doctor,Patient")]
    public async Task<IActionResult> GetMyRecords()
    {
        var userId = _userManager.GetUserId(User);
        var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        if (role == "Patient")
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null) return NotFound("Patient not found");

            var records = await _context.MedicalRecords
                .Where(r => r.PatientId == patient.PatientId)
                .ToListAsync();

            return Ok(records);
        }
        else if (role == "Doctor")
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null) return NotFound("Doctor not found");

            var records = await _context.MedicalRecords
                .Where(r => r.DoctorId == doctor.DoctorId)
                .ToListAsync();

            return Ok(records);
        }

        return Forbid();
    }
}
