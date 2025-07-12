using NeighborhoodServices.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ServiceBooking
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Service")]
    public int ServiceId { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    public DateTimeOffset ScheduledTime { get; set; }

    [Required]
    public string Status { get; set; } = "Pending";

    // ✅ Make these optional for POST binding
    public Service? Service { get; set; }
    public User? Customer { get; set; }
}
