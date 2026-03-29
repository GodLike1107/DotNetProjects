using MagicBricks.API.DTOs;
using MagicBricks.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MagicBricks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginatedResponse<PropertyResponseDto>>> Search(
        [FromQuery] PropertySearchDto searchDto)
    {
        var sessionId = GetSessionId();
        var result = await _propertyService.SearchPropertiesAsync(searchDto, sessionId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropertyResponseDto>> GetById(int id)
    {
        var sessionId = GetSessionId();
        var property = await _propertyService.GetPropertyByIdAsync(id, sessionId);
        if (property == null)
            return NotFound(new { message = "Property not found" });
        return Ok(property);
    }

    [HttpGet("cities")]
    public async Task<ActionResult<List<string>>> GetCities()
    {
        var cities = await _propertyService.GetCitiesAsync();
        return Ok(cities);
    }

    [HttpGet("localities/{city}")]
    public async Task<ActionResult<List<string>>> GetLocalities(string city)
    {
        var localities = await _propertyService.GetLocalitiesAsync(city);
        return Ok(localities);
    }

    // ===== FAVORITES =====

    [HttpPost("favorites/{propertyId:int}")]
    public async Task<ActionResult> ToggleFavorite(int propertyId)
    {
        var sessionId = GetOrCreateSessionId();
        var isFavorited = await _propertyService.ToggleFavoriteAsync(sessionId, propertyId);
        return Ok(new { isFavorited, message = isFavorited ? "Added to favorites" : "Removed from favorites" });
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<List<PropertyResponseDto>>> GetFavorites()
    {
        var sessionId = GetSessionId();
        if (string.IsNullOrWhiteSpace(sessionId))
            return Ok(new List<PropertyResponseDto>());

        var favorites = await _propertyService.GetFavoritesAsync(sessionId);
        return Ok(favorites);
    }

    // ===== CONTACT =====

    [HttpPost("contact")]
    public async Task<ActionResult> SubmitContact([FromBody] ContactInquiryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _propertyService.SubmitContactInquiryAsync(dto);
        if (!success)
            return NotFound(new { message = "Property not found" });

        return Ok(new { message = "Inquiry submitted successfully" });
    }

    // ===== HELPERS =====

    private string? GetSessionId()
    {
        return Request.Headers.TryGetValue("X-Session-Id", out var sessionId)
            ? sessionId.ToString()
            : null;
    }

    private string GetOrCreateSessionId()
    {
        var sessionId = GetSessionId();
        if (string.IsNullOrWhiteSpace(sessionId))
            sessionId = Guid.NewGuid().ToString();
        return sessionId;
    }
}
