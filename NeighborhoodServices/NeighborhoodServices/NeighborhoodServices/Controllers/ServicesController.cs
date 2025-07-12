using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeighborhoodServices.API.Data;
using NeighborhoodServices.DTOs;

namespace NeighborhoodServices.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly NeighborhoodDbContext _context;

        public ServicesController(NeighborhoodDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    Category = s.Category,
                    ProviderName = s.Provider.Name
                })
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
        {
            var service = await _context.Services
                .Include(s => s.Provider)
                .Where(s => s.Id == id)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    Category = s.Category,
                    ProviderName = s.Provider.Name
                })
                .FirstOrDefaultAsync();

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }



        [HttpGet("debug")]
        public async Task<IActionResult> DebugServices()
        {
            var services = await _context.Services.Include(s => s.Provider).ToListAsync();
            return Ok(services); // No DTO, raw entities
        }

    }
}