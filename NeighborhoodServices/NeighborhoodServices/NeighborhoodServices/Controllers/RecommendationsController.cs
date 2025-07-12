using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeighborhoodServices.API.Data;
using NeighborhoodServices.API.Models;

namespace NeighborhoodServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")] // Only customers should get recommendations
    public class RecommendationsController : ControllerBase
    {
        private readonly NeighborhoodDbContext _context;

        public RecommendationsController(NeighborhoodDbContext context)
        {
            _context = context;
        }

        // GET: api/recommendations/{customerId}
        [HttpGet("{customerId}")]
        public async Task<ActionResult<List<Service>>> GetRecommendations(int customerId)
        {
            Console.WriteLine("🧠 GetRecommendations endpoint HIT for CustomerId: " + customerId);
            // ✅ TEMPORARY DEBUGGING: Log all services and their categories
            var allServices = await _context.Services.ToListAsync();
            foreach (var s in allServices)
            {
                Console.WriteLine($"Id: {s.Id}, Title: {s.Title}, Category: {s.Category}");
            }

            // Step 1: Get all booked service IDs for this customer
            var bookedServiceIds = await _context.ServiceBookings
                .Where(b => b.CustomerId == customerId)
                .Select(b => b.ServiceId)
                .Distinct()
                .ToListAsync();

            // Step 2: Get the categories of the booked services
            var bookedCategories = await _context.Services
                .Where(s => bookedServiceIds.Contains(s.Id))
                .Select(s => s.Category)
                .Distinct()
                .ToListAsync();

            // ✅ Debugging categories
            Console.WriteLine("Booked Categories: " + string.Join(", ", bookedCategories));

            if (!bookedCategories.Any())
                return Ok(new List<Service>());

            // Step 3: Recommend other services in the same categories (excluding already booked)
            var recommended = await _context.Services
                .Include(s => s.Provider)
                .Where(s => bookedCategories.Contains(s.Category) && !bookedServiceIds.Contains(s.Id))
                .OrderByDescending(s => s.Title)
                .Take(5)
                .ToListAsync();

            return Ok(recommended);
        }
    }
}
