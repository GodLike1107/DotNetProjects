namespace MagicBricks.API.DTOs;

public class PropertyResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PropertyType { get; set; } = string.Empty;
    public string ListingType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FormattedPrice { get; set; } = string.Empty;
    public int Area { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public string City { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Furnishing { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime PostedDate { get; set; }
    public bool IsFavorited { get; set; }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
