using MagicBricks.API.DTOs;
using MagicBricks.API.Models;

namespace MagicBricks.API.Services;

public interface IPropertyService
{
    Task<PaginatedResponse<PropertyResponseDto>> SearchPropertiesAsync(PropertySearchDto searchDto, string? sessionId);
    Task<PropertyResponseDto?> GetPropertyByIdAsync(int id, string? sessionId);
    Task<List<string>> GetCitiesAsync();
    Task<List<string>> GetLocalitiesAsync(string city);

    // Favorites
    Task<bool> ToggleFavoriteAsync(string sessionId, int propertyId);
    Task<List<PropertyResponseDto>> GetFavoritesAsync(string sessionId);

    // Contact
    Task<bool> SubmitContactInquiryAsync(ContactInquiryDto dto);
}
