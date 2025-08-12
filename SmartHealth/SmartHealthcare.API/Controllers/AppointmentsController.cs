using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.DTOs;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Core.Enums;
using SmartHealthcare.Infrastructure.Data;

namespace SmartHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Book appointment (only for Patients)
        [HttpPost("book")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment(AppointmentDto model)
        {
            var userId = _userManager.GetUserId(User);

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                return NotFound("Patient not found");

            // 🔍 ✅ Check if doctor is available at that date/time
            var availabilityExists = await _context.DoctorAvailabilities.AnyAsync(a =>
                a.DoctorId == model.DoctorId &&
                a.AvailableDate == model.ScheduledTime.Date &&
                a.StartTime <= model.ScheduledTime.TimeOfDay &&
                a.EndTime > model.ScheduledTime.TimeOfDay
            );

            if (!availabilityExists)
                return BadRequest("Doctor is not available at this time.");

            // ⛔ Check doctor double booking
            bool isDoctorBooked = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == model.DoctorId &&
                a.ScheduledTime == model.ScheduledTime &&
                a.Status == AppointmentStatus.Booked
            );

            if (isDoctorBooked)
                return Conflict("Doctor is already booked at this time.");

            // ✅ Create appointment
            var appointment = new Appointment
            {
                DoctorId = model.DoctorId,
                PatientId = patient.PatientId,
                ScheduledTime = model.ScheduledTime,
                Status = model.Status,
                Notes = model.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment booked successfully!" });
        }

        // ✅ View my appointments (Patient)
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = _userManager.GetUserId(User);

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                return NotFound("Patient not found");

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patient.PatientId)
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ScheduledTime,
                    Status = a.Status.ToString(),
                    a.Notes,
                    DoctorName = a.Doctor.User.FullName,
                    DoctorEmail = a.Doctor.User.Email,
                    a.Doctor.Specialization
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // ✅ View doctor’s appointments (Doctor)
        [HttpGet("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var userId = _userManager.GetUserId(User);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
                return NotFound("Doctor not found");

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ScheduledTime,
                    Status = a.Status.ToString(),
                    PatientName = a.Patient.User.FullName,
                    PatientEmail = a.Patient.User.Email,
                    a.Patient.MedicalHistory
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // ✅ Admin: View all appointments
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ScheduledTime,
                    Status = a.Status.ToString(),
                    a.Notes,
                    Doctor = new
                    {
                        a.Doctor.DoctorId,
                        a.Doctor.User.FullName,
                        a.Doctor.Specialization
                    },
                    Patient = new
                    {
                        a.Patient.PatientId,
                        a.Patient.User.FullName,
                        a.Patient.MedicalHistory
                    }
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // ✅ Admin/Doctor/Patient: Update appointment status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] UpdateAppointmentDto model)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled || appointment.Status == AppointmentStatus.Completed)
                return BadRequest($"Cannot modify a {appointment.Status.ToString().ToLower()} appointment.");

            switch (model.Status)
            {
                case AppointmentStatus.Cancelled:
                    appointment.Status = AppointmentStatus.Cancelled;
                    break;

                case AppointmentStatus.Rescheduled:
                    if (!model.NewScheduledTime.HasValue)
                        return BadRequest("NewScheduledTime is required for rescheduling.");

                    bool conflict = await _context.Appointments.AnyAsync(a =>
                        a.DoctorId == appointment.DoctorId &&
                        a.ScheduledTime == model.NewScheduledTime.Value &&
                        a.AppointmentId != appointment.AppointmentId &&
                        a.Status == AppointmentStatus.Booked);

                    if (conflict)
                        return Conflict("Doctor is already booked at the new time.");

                    appointment.ScheduledTime = model.NewScheduledTime.Value;
                    appointment.Status = AppointmentStatus.Rescheduled;
                    break;

                case AppointmentStatus.Completed:
                    appointment.Status = AppointmentStatus.Completed;
                    break;

                default:
                    return BadRequest("Invalid status.");
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Appointment status updated." });
        }

        // ✅ Admin: Filter by status
        [HttpGet("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAppointmentsByStatus([FromQuery] string value)
        {
            if (!Enum.TryParse<AppointmentStatus>(value, true, out var statusEnum))
                return BadRequest("Invalid status");

            var appointments = await _context.Appointments
                .Where(a => a.Status == statusEnum)
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ScheduledTime,
                    Status = a.Status.ToString(),
                    a.Notes,
                    Doctor = new
                    {
                        a.Doctor.DoctorId,
                        a.Doctor.User.FullName,
                        a.Doctor.Specialization
                    },
                    Patient = new
                    {
                        a.Patient.PatientId,
                        a.Patient.User.FullName,
                        a.Patient.MedicalHistory
                    }
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // ✅ Optional: Filter by status, doctor, or date
        [HttpGet("filter")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> FilterAppointments([FromQuery] string? status, [FromQuery] int? doctorId, [FromQuery] DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<AppointmentStatus>(status, true, out var statusEnum))
                query = query.Where(a => a.Status == statusEnum);

            if (doctorId.HasValue)
                query = query.Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
                query = query.Where(a => a.ScheduledTime.Date == date.Value.Date);

            var result = await query.Select(a => new
            {
                a.AppointmentId,
                a.ScheduledTime,
                Status = a.Status.ToString(),
                a.Notes,
                Doctor = new
                {
                    a.Doctor.DoctorId,
                    a.Doctor.User.FullName,
                    a.Doctor.Specialization
                },
                Patient = new
                {
                    a.Patient.PatientId,
                    a.Patient.User.FullName,
                    a.Patient.MedicalHistory
                }
            }).ToListAsync();

            return Ok(result);
        }
    }
}