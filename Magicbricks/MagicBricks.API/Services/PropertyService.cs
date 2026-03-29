using MagicBricks.API.Data;
using MagicBricks.API.DTOs;
using MagicBricks.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicBricks.API.Services;

public class PropertyService : IPropertyService
{
    private readonly ApplicationDbContext _context;

    public PropertyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponse<PropertyResponseDto>> SearchPropertiesAsync(PropertySearchDto searchDto, string? sessionId)
    {
        var query = _context.Properties.Where(p => p.IsAvailable);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchDto.City))
            query = query.Where(p => p.City.ToLower() == searchDto.City.ToLower());

        if (!string.IsNullOrWhiteSpace(searchDto.ListingType))
            query = query.Where(p => p.ListingType.ToLower() == searchDto.ListingType.ToLower());

        if (searchDto.MinBudget.HasValue)
            query = query.Where(p => p.Price >= searchDto.MinBudget.Value);

        if (searchDto.MaxBudget.HasValue)
            query = query.Where(p => p.Price <= searchDto.MaxBudget.Value);

        if (searchDto.Bedrooms.HasValue)
            query = query.Where(p => p.Bedrooms == searchDto.Bedrooms.Value);

        if (!string.IsNullOrWhiteSpace(searchDto.PropertyType))
            query = query.Where(p => p.PropertyType.ToLower() == searchDto.PropertyType.ToLower());

        if (!string.IsNullOrWhiteSpace(searchDto.Furnishing))
            query = query.Where(p => p.Furnishing != null && p.Furnishing.ToLower() == searchDto.Furnishing.ToLower());

        if (!string.IsNullOrWhiteSpace(searchDto.SearchText))
        {
            var search = searchDto.SearchText.ToLower();
            query = query.Where(p =>
                p.Title.ToLower().Contains(search) ||
                p.Locality.ToLower().Contains(search) ||
                (p.Description != null && p.Description.ToLower().Contains(search)));
        }

        // Apply sorting
        query = searchDto.SortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "date_desc" => query.OrderByDescending(p => p.PostedDate),
            "area_asc" => query.OrderBy(p => p.Area),
            "area_desc" => query.OrderByDescending(p => p.Area),
            _ => query.OrderByDescending(p => p.PostedDate)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToListAsync();

        var favoritePropertyIds = new HashSet<int>();
        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            favoritePropertyIds = (await _context.Favorites
                .Where(f => f.SessionId == sessionId)
                .Select(f => f.PropertyId)
                .ToListAsync())
                .ToHashSet();
        }

        return new PaginatedResponse<PropertyResponseDto>
        {
            Items = items.Select(p => MapToDto(p, favoritePropertyIds.Contains(p.Id))).ToList(),
            TotalCount = totalCount,
            Page = searchDto.Page,
            PageSize = searchDto.PageSize
        };
    }

    public async Task<PropertyResponseDto?> GetPropertyByIdAsync(int id, string? sessionId)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property == null) return null;

        var isFavorited = false;
        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            isFavorited = await _context.Favorites
                .AnyAsync(f => f.SessionId == sessionId && f.PropertyId == id);
        }

        return MapToDto(property, isFavorited);
    }

    public async Task<List<string>> GetCitiesAsync()
    {
        return await _context.Properties
            .Select(p => p.City)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<List<string>> GetLocalitiesAsync(string city)
    {
        return await _context.Properties
            .Where(p => p.City.ToLower() == city.ToLower())
            .Select(p => p.Locality)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();
    }

    public async Task<bool> ToggleFavoriteAsync(string sessionId, int propertyId)
    {
        var existing = await _context.Favorites
            .FirstOrDefaultAsync(f => f.SessionId == sessionId && f.PropertyId == propertyId);

        if (existing != null)
        {
            _context.Favorites.Remove(existing);
            await _context.SaveChangesAsync();
            return false; // Removed from favorites
        }

        _context.Favorites.Add(new Favorite
        {
            SessionId = sessionId,
            PropertyId = propertyId
        });
        await _context.SaveChangesAsync();
        return true; // Added to favorites
    }

    public async Task<List<PropertyResponseDto>> GetFavoritesAsync(string sessionId)
    {
        var favorites = await _context.Favorites
            .Where(f => f.SessionId == sessionId)
            .Include(f => f.Property)
            .Where(f => f.Property != null)
            .Select(f => f.Property!)
            .ToListAsync();

        return favorites.Select(p => MapToDto(p, true)).ToList();
    }

    public async Task<bool> SubmitContactInquiryAsync(ContactInquiryDto dto)
    {
        var property = await _context.Properties.FindAsync(dto.PropertyId);
        if (property == null) return false;

        _context.ContactInquiries.Add(new ContactInquiry
        {
            PropertyId = dto.PropertyId,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Message = dto.Message
        });

        await _context.SaveChangesAsync();
        return true;
    }

    private static PropertyResponseDto MapToDto(Property p, bool isFavorited)
    {
        return new PropertyResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            PropertyType = p.PropertyType,
            ListingType = p.ListingType,
            Price = p.Price,
            FormattedPrice = FormatPrice(p.Price, p.ListingType),
            Area = p.Area,
            Bedrooms = p.Bedrooms,
            Bathrooms = p.Bathrooms,
            City = p.City,
            Locality = p.Locality,
            Address = p.Address,
            Furnishing = p.Furnishing,
            ImageUrl = p.ImageUrl,
            IsAvailable = p.IsAvailable,
            PostedDate = p.PostedDate,
            IsFavorited = isFavorited
        };
    }

    private static string FormatPrice(decimal price, string listingType)
    {
        if (listingType.Equals("Rent", StringComparison.OrdinalIgnoreCase))
        {
            return $"₹{price:N0}/mo";
        }

        if (price >= 10000000) // 1 Cr+
        {
            var crores = price / 10000000;
            return crores % 1 == 0 ? $"₹{crores:N0} Cr" : $"₹{crores:N1} Cr";
        }
        else if (price >= 100000) // 1 Lac+
        {
            var lacs = price / 100000;
            return lacs % 1 == 0 ? $"₹{lacs:N0} Lac" : $"₹{lacs:N1} Lac";
        }

        return $"₹{price:N0}";
    }
}
