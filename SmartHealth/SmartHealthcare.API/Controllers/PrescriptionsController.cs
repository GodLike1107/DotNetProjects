using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Application.Services;
using SmartHealthcare.Core.DTOs;
using SmartHealthcare.Infrastructure.Data; // Namespace where AppDbContext is defined
using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading.Tasks;

namespace SmartHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly PrescriptionService _service;
        private readonly AppDbContext _context; // ✅ Added DbContext

        public PrescriptionsController(PrescriptionService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // ------------------ CREATE PRESCRIPTION ------------------
        [HttpPost("create")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreatePrescription(PrescriptionDto dto)
        {
            // 1️⃣ Get the logged-in user's ID from JWT (string, not Guid)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2️⃣ Lookup the doctor's integer ID from DB
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                return BadRequest(new { message = "Doctor not found" });

            // 3️⃣ Call service with the correct int DoctorId
            var prescription = await _service.CreatePrescriptionAsync(doctor.DoctorId, dto);

            return Ok(prescription);
        }

        // ------------------ GET PRESCRIPTIONS FOR LOGGED-IN PATIENT ------------------
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyPrescriptions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get PatientId from DB
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                return BadRequest(new { message = "Patient not found" });

            var prescriptions = await _service.GetPrescriptionsForPatientAsync(patient.PatientId);
            return Ok(prescriptions);
        }

        // ------------------ GET PRESCRIPTION BY ID ------------------
        [HttpGet("{id}")]
        [Authorize] // doctor or patient can view by ID
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            var prescription = await _service.GetPrescriptionByIdAsync(id);
            if (prescription == null)
                return NotFound(new { message = "Prescription not found" });

            return Ok(prescription);
        }

        // ------------------ DOWNLOAD PRESCRIPTION AS PDF ------------------
        [HttpGet("{id}/pdf")]
        [Authorize] // doctor or patient can download by ID
        public async Task<IActionResult> GetPrescriptionPdf(int id)
        {
            var prescription = await _service.GetPrescriptionByIdAsync(id);
            if (prescription == null)
                return NotFound(new { message = "Prescription not found" });

            using (var ms = new MemoryStream())
            {
                // Create PDF
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new Paragraph("---- Prescription Details ----"));
                doc.Add(new Paragraph($"Prescription ID: {prescription.PrescriptionId}"));
                doc.Add(new Paragraph($"Doctor ID: {prescription.DoctorId}"));
                doc.Add(new Paragraph($"Patient ID: {prescription.PatientId}"));
                doc.Add(new Paragraph($"Medication Details: {prescription.MedicationDetails}"));
                doc.Add(new Paragraph($"Notes: {prescription.Notes}"));
                doc.Add(new Paragraph($"Date Issued: {prescription.CreatedAt:yyyy-MM-dd}"));

                doc.Close();

                return File(ms.ToArray(), "application/pdf", $"Prescription_{id}.pdf");
            }
        }
    }
}
