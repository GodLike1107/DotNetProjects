using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeighborhoodServices.API.Data;
using NeighborhoodServices.API.Models;
using NeighborhoodServices.DTOs;
using System.Security.Claims;

namespace NeighborhoodServices.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly NeighborhoodDbContext _context;

    public BookingsController(NeighborhoodDbContext context)
    {
        _context = context;
    }

    // ✅ Helper methods
    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    private string GetUserEmail()
    {
        var emailClaim = User.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.Email ||
            c.Type == "email" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        );
        return emailClaim?.Value ?? "";
    }

    private string GetUserRole() =>
        User.FindFirst(ClaimTypes.Role)?.Value ?? "";

    // ✅ GET: api/bookings/my (Customer bookings)
    [Authorize(Roles = "Customer")]
    [HttpGet("my")]
    public IActionResult GetMyBookings()
    {
        var userId = GetUserId();

        var bookings = _context.ServiceBookings
            .Include(b => b.Service)
                .ThenInclude(s => s.Provider)
            .Where(b => b.CustomerId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                ServiceTitle = b.Service.Title,
                ProviderName = b.Service.Provider.Name,
                ScheduledTime = b.ScheduledTime,
                Status = b.Status
            })
            .ToList();

        return Ok(bookings); // 👈 returns an array
    }

    // ✅ GET: api/bookings/provider (Provider bookings)
    [Authorize(Roles = "Provider")]
    [HttpGet("provider")]
    public async Task<ActionResult<List<BookingDto>>> GetProviderBookings()
    {
        var providerId = GetUserId();

        var bookings = await _context.ServiceBookings
            .Include(b => b.Customer)
            .Include(b => b.Service)
                .ThenInclude(s => s.Provider)
            .Where(b => b.Service.ProviderId == providerId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                ServiceTitle = b.Service.Title,
                ProviderName = b.Service.Provider.Name,
                ScheduledTime = b.ScheduledTime,
                Status = b.Status,
                CustomerName = b.Customer.Name,
                CustomerEmail = b.Customer.Email
            })
            .ToListAsync();

        return Ok(bookings); // 👈 returns a list (array)
    }

    // ✅ GET: api/bookings (Generic bookings list - internal usage)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetBookings()
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var query = _context.ServiceBookings
            .Include(b => b.Service)
                .ThenInclude(s => s.Provider)
            .Include(b => b.Customer)
            .AsQueryable();

        if (role == "Customer")
            query = query.Where(b => b.CustomerId == userId);
        else if (role == "Provider")
            query = query.Where(b => b.Service.ProviderId == userId);
        else
            return Forbid("Access denied");

        var bookings = await query
            .Select(b => new
            {
                b.Id,
                b.ScheduledTime,
                b.Status,
                b.ServiceId,
                b.CustomerId,
                Service = new
                {
                    b.Service.Title,
                    b.Service.Category,
                    Provider = new { b.Service.Provider.Name }
                },
                Customer = new
                {
                    b.Customer.Name,
                    b.Customer.Email
                }
            })
            .ToListAsync();

        return Ok(bookings);
    }

    // ✅ GET: api/bookings/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetBooking(int id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var booking = await _context.ServiceBookings
            .Include(b => b.Service).ThenInclude(s => s.Provider)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            return NotFound();

        if (role == "Customer" && booking.CustomerId != userId)
            return Forbid();

        if (role == "Provider" && booking.Service.ProviderId != userId)
            return Forbid();

        return Ok(new
        {
            booking.Id,
            booking.ScheduledTime,
            booking.Status,
            booking.ServiceId,
            booking.CustomerId,
            Service = new
            {
                booking.Service.Title,
                booking.Service.Category,
                Provider = new { booking.Service.Provider.Name }
            },
            Customer = new
            {
                booking.Customer.Name,
                booking.Customer.Email
            }
        });
    }

    // ✅ POST: api/bookings
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateBooking([FromBody] ServiceBooking booking)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userEmail = GetUserEmail();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
            return NotFound("User not found");

        var serviceExists = await _context.Services.AnyAsync(s => s.Id == booking.ServiceId);
        if (!serviceExists)
            return BadRequest(new { message = "Selected service does not exist." });

        booking.CustomerId = user.Id;

        var exists = await _context.ServiceBookings.AnyAsync(b =>
            b.CustomerId == booking.CustomerId &&
            b.ServiceId == booking.ServiceId &&
            b.ScheduledTime == booking.ScheduledTime);

        if (exists)
            return BadRequest(new { message = "You already have a booking for this service at the selected time." });

        booking.Status = "Confirmed";
        _context.ServiceBookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, new
        {
            booking.Id,
            booking.ScheduledTime,
            booking.Status,
            booking.ServiceId,
            booking.CustomerId
        });
    }

    // ✅ PUT: api/bookings/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, ServiceBooking updatedBooking)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        if (id != updatedBooking.Id)
            return BadRequest();

        var existing = await _context.ServiceBookings
            .Include(b => b.Service)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existing == null)
            return NotFound();

        if (role == "Customer" && existing.CustomerId != userId)
            return Forbid();

        if (role == "Provider" && existing.Service.ProviderId != userId)
            return Forbid();

        existing.ScheduledTime = updatedBooking.ScheduledTime;
        existing.Status = updatedBooking.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ✅ DELETE: api/bookings/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var booking = await _context.ServiceBookings
            .Include(b => b.Service)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            return NotFound();

        if (role == "Customer" && booking.CustomerId != userId)
            return Forbid();

        if (role == "Provider" && booking.Service.ProviderId != userId)
            return Forbid();

        _context.ServiceBookings.Remove(booking);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
