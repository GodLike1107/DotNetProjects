using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcare.Core.Entities;
using SmartHealthcare.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _context;

    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitFeedback(Feedback feedback)
    {
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();
        return Ok(feedback);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorFeedback(int doctorId)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.DoctorId == doctorId)
            .ToListAsync();

        var averageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;

        return Ok(new
        {
            AverageRating = averageRating,
            Reviews = feedbacks
        });
    }
}
