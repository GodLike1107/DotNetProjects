using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicBricks.API.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required, MaxLength(50)]
    public string PropertyType { get; set; } = string.Empty; // Apartment, Villa, Independent House, Studio, Penthouse

    [Required, MaxLength(10)]
    public string ListingType { get; set; } = string.Empty; // Buy or Rent

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int Area { get; set; } // Square feet

    [Required]
    public int Bedrooms { get; set; }

    [Required]
    public int Bathrooms { get; set; }

    [Required, MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Locality { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? Furnishing { get; set; } // Furnished, Semi-Furnished, Unfurnished

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
