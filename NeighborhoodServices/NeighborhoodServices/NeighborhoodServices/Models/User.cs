using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeighborhoodServices.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } // "Customer", "Provider"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ICollection<Service> ServicesOffered { get; set; } = new List<Service>();
        public ICollection<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
    }
}