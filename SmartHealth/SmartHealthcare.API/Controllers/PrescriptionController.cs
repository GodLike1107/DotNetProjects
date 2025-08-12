using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly AppDbContext _context;

    public PrescriptionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return Ok(prescription);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescription(int id)
    {
        var prescription = await _context.Prescriptions
            .Include(p => p.Doctor)
            .Include(p => p.Patient)
            .FirstOrDefaultAsync(p => p.PrescriptionId == id);

        if (prescription == null)
            return NotFound();

        return Ok(prescription);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadPrescription(int id)
    {
        var prescription = await _context.Prescriptions.FindAsync(id);
        if (prescription == null || string.IsNullOrEmpty(prescription.FileUrl))
            return NotFound("PDF not found.");

        var filePath = prescription.FileUrl;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/pdf", $"Prescription_{id}.pdf");
    }
}
