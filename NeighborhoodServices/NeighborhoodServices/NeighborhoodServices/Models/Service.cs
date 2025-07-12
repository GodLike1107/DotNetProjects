using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeighborhoodServices.API.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required, MaxLength(50)]
        public string Category { get; set; }

        [ForeignKey("Provider")]
        public int ProviderId { get; set; }

        // Navigation properties
        public User Provider { get; set; }
        public ICollection<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
    }
}