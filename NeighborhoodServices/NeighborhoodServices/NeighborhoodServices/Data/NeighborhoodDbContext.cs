using Microsoft.EntityFrameworkCore;
using NeighborhoodServices.API.Models;

namespace NeighborhoodServices.API.Data
{
    public class NeighborhoodDbContext : DbContext
    {
        public NeighborhoodDbContext(DbContextOptions<NeighborhoodDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceBooking> ServiceBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for Price
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(10, 2); // 10 digits total, 2 decimal places

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Service entity relationships
            modelBuilder.Entity<Service>()
                .HasOne(s => s.Provider)
                .WithMany(u => u.ServicesOffered)
                .HasForeignKey(s => s.ProviderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent circular cascade issues

            modelBuilder.Entity<Service>()
                .HasIndex(s => new { s.Title, s.ProviderId })
                .IsUnique();

            // Configure ServiceBooking entity relationships
            modelBuilder.Entity<ServiceBooking>()
                .HasOne(sb => sb.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(sb => sb.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceBooking>()
                .HasOne(sb => sb.Customer)
                .WithMany(u => u.Bookings)
                .HasForeignKey(sb => sb.CustomerId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade loop

            // Unique constraint to prevent duplicate bookings
            modelBuilder.Entity<ServiceBooking>()
                .HasIndex(b => new { b.ServiceId, b.CustomerId, b.ScheduledTime })
                .IsUnique();

            // Seed sample data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Users with passwords (plaintext for now — replace with hashed in production)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "John Provider",
                    Email = "john@example.com",
                    Role = "Provider",
                    Password = "123456"
                },
                new User
                {
                    Id = 2,
                    Name = "Jane Customer",
                    Email = "jane@example.com",
                    Role = "Customer",
                    Password = "password"
                }
            );

            // Services
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Title = "Dog Walking",
                    Description = "Professional dog walking service in your neighborhood",
                    Price = 25.00m,
                    Category = "Pet Care",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 2,
                    Title = "Math Tutoring",
                    Description = "High school and college level math tutoring",
                    Price = 40.00m,
                    Category = "Education",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 3,
                    Title = "Yoga Classes",
                    Description = "Join our guided yoga sessions for better health",
                    Price = 35.00m,
                    Category = "Fitness",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 4,
                    Title = "House Cleaning",
                    Description = "Thorough home cleaning with eco-friendly supplies",
                    Price = 50.00m,
                    Category = "Home Services",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 5,
                    Title = "Haircut & Grooming",
                    Description = "Professional grooming service at your home",
                    Price = 30.00m,
                    Category = "Beauty",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 6,
                    Title = "Plumbing Repairs",
                    Description = "Fix leaks and broken pipes with our expert plumber",
                    Price = 60.00m,
                    Category = "Maintenance",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 7,
                    Title = "Mobile Phone Repair",
                    Description = "Get your phone fixed at home by a technician",
                    Price = 45.00m,
                    Category = "Tech Support",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 8,
                    Title = "Piano Lessons",
                    Description = "One-on-one beginner piano instruction",
                    Price = 55.00m,
                    Category = "Education",
                    ProviderId = 1
                },
                new Service
                {
                    Id = 9,
                    Title = "Laundry Service",
                    Description = "Pickup and delivery laundry wash & fold",
                    Price = 20.00m,
                    Category = "Home Services",
                    ProviderId = 1
                }
               
            );
        }
    }
}
