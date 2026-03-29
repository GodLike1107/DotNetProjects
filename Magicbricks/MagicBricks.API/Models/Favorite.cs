using System.ComponentModel.DataAnnotations;

namespace MagicBricks.API.Models;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string SessionId { get; set; } = string.Empty; // Browser session-based (no auth)

    [Required]
    public int PropertyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Property? Property { get; set; }
}
