namespace MagicBricks.API.DTOs;

public class PropertySearchDto
{
    public string? City { get; set; }
    public string? ListingType { get; set; } // Buy or Rent
    public decimal? MinBudget { get; set; }
    public decimal? MaxBudget { get; set; }
    public int? Bedrooms { get; set; }
    public string? PropertyType { get; set; }
    public string? SearchText { get; set; }
    public string? Furnishing { get; set; }
    public string? SortBy { get; set; } // price_asc, price_desc, date_desc, area_asc, area_desc
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
