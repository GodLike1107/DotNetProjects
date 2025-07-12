using Microsoft.AspNetCore.Mvc;
using NeighborhoodServices.DTOs;
using System.Collections.Generic;

namespace NeighborhoodServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NeighborhoodsController : ControllerBase
    {
        // In-memory store (simulated DB)
        private static List<string> neighborhoods = new List<string>
        {
            "HSR Layout",
            "Indiranagar",
            "BTM Layout"
        };

        // GET: api/neighborhoods
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(neighborhoods);
        }

        // POST: api/neighborhoods
        [HttpPost]
        public ActionResult<List<string>> Post([FromBody] NeighborhoodDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            neighborhoods.Add(dto.Name);
            return Ok(neighborhoods);
        }

    }
}
