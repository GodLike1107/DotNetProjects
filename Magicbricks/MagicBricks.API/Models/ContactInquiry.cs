using System.ComponentModel.DataAnnotations;

namespace MagicBricks.API.Models;

public class ContactInquiry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PropertyId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }

    [MaxLength(1000)]
    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Property? Property { get; set; }
}
