using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.DTOs;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;


namespace SmartHealthcare.Application.Services
{
    public class PrescriptionService
    {
        private readonly AppDbContext _context;

        public PrescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionResponseDto> CreatePrescriptionAsync(int doctorId, PrescriptionDto dto)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId && a.DoctorId == doctorId);

            if (appointment == null)
                throw new Exception("Appointment not found or unauthorized.");

            // 1. Create prescription entity
            var prescription = new Prescription
            {
                AppointmentId = dto.AppointmentId,
                DoctorId = doctorId,
                PatientId = appointment.PatientId,
                MedicationDetails = dto.MedicationDetails,
                Notes = dto.Notes
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            // 2. Generate PDF file
            var pdfFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "prescriptions");
            if (!Directory.Exists(pdfFolder))
                Directory.CreateDirectory(pdfFolder);

            var pdfPath = Path.Combine(pdfFolder, $"Prescription_{prescription.PrescriptionId}.pdf");

            using (var stream = new FileStream(pdfPath, FileMode.Create))
            {
                var doc = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
                doc.Open();

                doc.Add(new iTextSharp.text.Paragraph($"Prescription ID: {prescription.PrescriptionId}"));
                doc.Add(new iTextSharp.text.Paragraph($"Doctor ID: {doctorId}"));
                doc.Add(new iTextSharp.text.Paragraph($"Patient ID: {appointment.PatientId}"));
                doc.Add(new iTextSharp.text.Paragraph($"Medication: {dto.MedicationDetails}"));
                doc.Add(new iTextSharp.text.Paragraph($"Notes: {dto.Notes}"));
                doc.Add(new iTextSharp.text.Paragraph($"Date: {DateTime.Now:yyyy-MM-dd HH:mm}"));

                doc.Close();
            }

            // 3. Store URL
            prescription.FileUrl = $"/files/prescriptions/Prescription_{prescription.PrescriptionId}.pdf";
            await _context.SaveChangesAsync();

            // 4. Return DTO
            return new PrescriptionResponseDto
            {
                PrescriptionId = prescription.PrescriptionId,
                AppointmentId = prescription.AppointmentId,
                DoctorId = prescription.DoctorId,
                PatientId = prescription.PatientId,
                MedicationDetails = prescription.MedicationDetails,
                Notes = prescription.Notes,
                CreatedAt = prescription.CreatedAt,
                FileUrl = prescription.FileUrl
            };
        }


        public async Task<IEnumerable<PrescriptionResponseDto>> GetPrescriptionsForPatientAsync(int patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .Select(p => new PrescriptionResponseDto
                {
                    PrescriptionId = p.PrescriptionId,
                    AppointmentId = p.AppointmentId,
                    DoctorId = p.DoctorId,
                    PatientId = p.PatientId,
                    MedicationDetails = p.MedicationDetails,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    FileUrl = p.FileUrl
                })
                .ToListAsync();
        }

        public async Task<PrescriptionResponseDto?> GetPrescriptionByIdAsync(int prescriptionId)
        {
            var p = await _context.Prescriptions
                .FirstOrDefaultAsync(x => x.PrescriptionId == prescriptionId);

            if (p == null) return null;

            return new PrescriptionResponseDto
            {
                PrescriptionId = p.PrescriptionId,
                AppointmentId = p.AppointmentId,
                DoctorId = p.DoctorId,
                PatientId = p.PatientId,
                MedicationDetails = p.MedicationDetails,
                Notes = p.Notes,
                CreatedAt = p.CreatedAt,
                FileUrl = p.FileUrl
            };
        }
    }
}