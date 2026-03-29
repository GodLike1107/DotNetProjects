using System.ComponentModel.DataAnnotations;

namespace MagicBricks.API.DTOs;

public class ContactInquiryDto
{
    [Required]
    public int PropertyId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Phone { get; set; }

    [MaxLength(1000)]
    public string? Message { get; set; }
}
