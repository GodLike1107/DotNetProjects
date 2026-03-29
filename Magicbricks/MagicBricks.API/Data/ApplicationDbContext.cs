using MagicBricks.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicBricks.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Property> Properties => Set<Property>();
    public DbSet<ContactInquiry> ContactInquiries => Set<ContactInquiry>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Favorite>()
            .HasIndex(f => new { f.SessionId, f.PropertyId })
            .IsUnique();

        modelBuilder.Entity<Property>().HasData(GetSeedProperties());
    }

    private static Property[] GetSeedProperties()
    {
        var now = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        return new Property[]
        {
            // ========== BANGALORE PROPERTIES ==========
            new Property
            {
                Id = 1,
                Title = "Luxurious 3BHK Apartment in Koramangala",
                Description = "Spacious 3BHK apartment with modern amenities in the heart of Koramangala. Close to IT hubs, restaurants, and shopping malls. Gated community with 24/7 security, swimming pool, gym, and children's play area. East-facing with ample natural light.",
                PropertyType = "Apartment",
                ListingType = "Buy",
                Price = 12000000m,
                Area = 1650,
                Bedrooms = 3,
                Bathrooms = 2,
                City = "Bangalore",
                Locality = "Koramangala",
                Address = "5th Block, Koramangala, Bangalore 560095",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-1.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-5),
                CreatedAt = now.AddDays(-5)
            },
            new Property
            {
                Id = 2,
                Title = "Modern 2BHK Flat in Whitefield",
                Description = "Well-maintained 2BHK flat in a premium gated community in Whitefield. Walking distance to ITPL and Phoenix Marketcity. Amenities include clubhouse, swimming pool, indoor games, and landscaped gardens.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 25000m,
                Area = 1100,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Bangalore",
                Locality = "Whitefield",
                Address = "EPIP Zone, Whitefield, Bangalore 560066",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-2.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-3),
                CreatedAt = now.AddDays(-3)
            },
            new Property
            {
                Id = 3,
                Title = "Premium 4BHK Villa in Jayanagar",
                Description = "Stunning independent villa with premium finishes in Jayanagar. Features include private garden, car parking for 2, modular kitchen, Italian marble flooring, and smart home automation. Located in a quiet residential neighborhood.",
                PropertyType = "Villa",
                ListingType = "Buy",
                Price = 25000000m,
                Area = 3200,
                Bedrooms = 4,
                Bathrooms = 4,
                City = "Bangalore",
                Locality = "Jayanagar",
                Address = "4th Block, Jayanagar, Bangalore 560041",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-3.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-10),
                CreatedAt = now.AddDays(-10)
            },
            new Property
            {
                Id = 4,
                Title = "Cozy 1BHK Studio in Indiranagar",
                Description = "Compact and stylish 1BHK studio apartment in the vibrant Indiranagar neighborhood. Perfect for young professionals. Walking distance to 100 Feet Road, Metro station, and trendy cafes. Fully furnished with modern appliances.",
                PropertyType = "Studio",
                ListingType = "Rent",
                Price = 18000m,
                Area = 550,
                Bedrooms = 1,
                Bathrooms = 1,
                City = "Bangalore",
                Locality = "Indiranagar",
                Address = "12th Main, Indiranagar, Bangalore 560038",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-4.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-2),
                CreatedAt = now.AddDays(-2)
            },
            new Property
            {
                Id = 5,
                Title = "Spacious 3BHK Independent House in HSR Layout",
                Description = "Beautiful independent house in HSR Layout with excellent connectivity. Features 3 bedrooms, modular kitchen, spacious living room, terrace garden, and covered car parking. Near Agara Lake and multiple tech parks.",
                PropertyType = "Independent House",
                ListingType = "Buy",
                Price = 18000000m,
                Area = 2200,
                Bedrooms = 3,
                Bathrooms = 3,
                City = "Bangalore",
                Locality = "HSR Layout",
                Address = "Sector 2, HSR Layout, Bangalore 560102",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-5.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-7),
                CreatedAt = now.AddDays(-7)
            },
            new Property
            {
                Id = 6,
                Title = "Affordable 2BHK Apartment in Electronic City",
                Description = "Well-designed 2BHK apartment in Electronic City Phase 1. Ideal for IT professionals working in Infosys/Wipro campuses. Gated society with power backup, gym, and play area. Excellent public transport connectivity.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 15000m,
                Area = 950,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Bangalore",
                Locality = "Electronic City",
                Address = "Phase 1, Electronic City, Bangalore 560100",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-6.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-1),
                CreatedAt = now.AddDays(-1)
            },
            new Property
            {
                Id = 7,
                Title = "Elegant 3BHK Flat in Marathahalli",
                Description = "Brand new 3BHK apartment in a high-rise tower in Marathahalli. Panoramic city views from the 15th floor. Close to Outer Ring Road, restaurants, and entertainment zones. Premium amenities including rooftop infinity pool.",
                PropertyType = "Apartment",
                ListingType = "Buy",
                Price = 8500000m,
                Area = 1450,
                Bedrooms = 3,
                Bathrooms = 2,
                City = "Bangalore",
                Locality = "Marathahalli",
                Address = "ORR Main Road, Marathahalli, Bangalore 560037",
                Furnishing = "Unfurnished",
                ImageUrl = "/assets/images/prop-7.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-12),
                CreatedAt = now.AddDays(-12)
            },
            new Property
            {
                Id = 8,
                Title = "Budget 1BHK Apartment in BTM Layout",
                Description = "Comfortable 1BHK apartment in BTM Layout Stage 2. Great location with easy access to Silk Board junction and surrounding IT corridors. Suitable for bachelors and small families. Well-maintained society.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 12000m,
                Area = 600,
                Bedrooms = 1,
                Bathrooms = 1,
                City = "Bangalore",
                Locality = "BTM Layout",
                Address = "Stage 2, BTM Layout, Bangalore 560076",
                Furnishing = "Unfurnished",
                ImageUrl = "/assets/images/prop-8.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-4),
                CreatedAt = now.AddDays(-4)
            },
            new Property
            {
                Id = 9,
                Title = "Ultra-Luxury 4BHK Villa in Sarjapur Road",
                Description = "Exquisite 4BHK villa in a premium villa community on Sarjapur Road. Private swimming pool, landscaped garden, home theater room, and Italian marble throughout. Clubhouse with world-class amenities. Close to international schools.",
                PropertyType = "Villa",
                ListingType = "Buy",
                Price = 32000000m,
                Area = 4200,
                Bedrooms = 4,
                Bathrooms = 5,
                City = "Bangalore",
                Locality = "Sarjapur Road",
                Address = "Sarjapur Main Road, Bangalore 562125",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-9.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-15),
                CreatedAt = now.AddDays(-15)
            },
            new Property
            {
                Id = 10,
                Title = "Elegant 2BHK Flat in Hebbal",
                Description = "Well-appointed 2BHK apartment near Hebbal Lake. Modern interiors with wooden flooring in bedrooms. Excellent connectivity to airport via Bellary Road. Society features include jogging track, tennis court, and meditation center.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 22000m,
                Area = 1200,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Bangalore",
                Locality = "Hebbal",
                Address = "Bellary Road, Hebbal, Bangalore 560024",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-10.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-6),
                CreatedAt = now.AddDays(-6)
            },

            // ========== MUMBAI PROPERTIES ==========
            new Property
            {
                Id = 11,
                Title = "Stunning 2BHK Flat in Andheri West",
                Description = "Beautifully designed 2BHK apartment in Andheri West near Lokhandwala. Close to DN Nagar Metro station, Infinity Mall, and Versova Beach. Features include modular kitchen, vitrified flooring, and video door phone.",
                PropertyType = "Apartment",
                ListingType = "Buy",
                Price = 15000000m,
                Area = 850,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Mumbai",
                Locality = "Andheri West",
                Address = "Lokhandwala Complex, Andheri West, Mumbai 400053",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-11.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-8),
                CreatedAt = now.AddDays(-8)
            },
            new Property
            {
                Id = 12,
                Title = "Premium 3BHK Apartment in Powai",
                Description = "Luxury 3BHK apartment overlooking Powai Lake in a renowned high-rise complex. World-class amenities including infinity pool, spa, concierge service, and business center. Walking distance to IIT Bombay and Hiranandani Gardens.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 45000m,
                Area = 1400,
                Bedrooms = 3,
                Bathrooms = 2,
                City = "Mumbai",
                Locality = "Powai",
                Address = "Hiranandani Gardens, Powai, Mumbai 400076",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-12.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-4),
                CreatedAt = now.AddDays(-4)
            },
            new Property
            {
                Id = 13,
                Title = "Iconic 4BHK Penthouse in Worli",
                Description = "Breathtaking 4BHK penthouse with panoramic sea views in Worli. Spread across 2 floors with private terrace, jacuzzi, and entertainment deck. Ultra-premium finishes with imported fixtures. 24/7 valet parking and concierge.",
                PropertyType = "Penthouse",
                ListingType = "Buy",
                Price = 55000000m,
                Area = 3500,
                Bedrooms = 4,
                Bathrooms = 4,
                City = "Mumbai",
                Locality = "Worli",
                Address = "Worli Sea Face, Mumbai 400018",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-13.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-20),
                CreatedAt = now.AddDays(-20)
            },
            new Property
            {
                Id = 14,
                Title = "Compact 1BHK Flat in Malad West",
                Description = "Well-maintained 1BHK flat in Malad West. Close to Inorbit Mall, Western Express Highway, and Malad Railway Station. Society with garden, covered parking, and 24/7 security. Ideal for working professionals.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 20000m,
                Area = 500,
                Bedrooms = 1,
                Bathrooms = 1,
                City = "Mumbai",
                Locality = "Malad West",
                Address = "Evershine Nagar, Malad West, Mumbai 400064",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-14.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-2),
                CreatedAt = now.AddDays(-2)
            },
            new Property
            {
                Id = 15,
                Title = "Spacious 3BHK Apartment in Thane West",
                Description = "Large 3BHK apartment in a reputed township in Thane West. Excellent connectivity via Eastern Express Highway and Ghodbunder Road. Township amenities include Olympic-size pool, cricket pitch, amphitheater, and jogging track.",
                PropertyType = "Apartment",
                ListingType = "Buy",
                Price = 11000000m,
                Area = 1350,
                Bedrooms = 3,
                Bathrooms = 2,
                City = "Mumbai",
                Locality = "Thane West",
                Address = "Ghodbunder Road, Thane West, Mumbai 400607",
                Furnishing = "Unfurnished",
                ImageUrl = "/assets/images/prop-15.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-9),
                CreatedAt = now.AddDays(-9)
            },
            new Property
            {
                Id = 16,
                Title = "Modern 2BHK Flat in Goregaon East",
                Description = "Newly renovated 2BHK flat in Goregaon East near Film City. Fully furnished with designer interiors. Close to Oberoi Mall, NESCO grounds, and Aarey Colony. Excellent connectivity by road and rail.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 30000m,
                Area = 900,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Mumbai",
                Locality = "Goregaon East",
                Address = "Film City Road, Goregaon East, Mumbai 400063",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-16.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-3),
                CreatedAt = now.AddDays(-3)
            },
            new Property
            {
                Id = 17,
                Title = "Exclusive 3BHK Villa in Juhu",
                Description = "Magnificent 3BHK sea-facing villa in prestigious Juhu. Private compound with lush landscaping, infinity pool, and entertainment pavilion. Walking distance to Juhu Beach. One of Mumbai's most coveted addresses.",
                PropertyType = "Villa",
                ListingType = "Buy",
                Price = 85000000m,
                Area = 4500,
                Bedrooms = 3,
                Bathrooms = 3,
                City = "Mumbai",
                Locality = "Juhu",
                Address = "Juhu Tara Road, Juhu, Mumbai 400049",
                Furnishing = "Furnished",
                ImageUrl = "/assets/images/prop-17.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-25),
                CreatedAt = now.AddDays(-25)
            },
            new Property
            {
                Id = 18,
                Title = "Value 1BHK Flat in Kandivali East",
                Description = "Affordable 1BHK flat in Kandivali East. Well-connected area with proximity to Western Express Highway and Kandivali Railway Station. Society with basic amenities, parking, and security. Best value for money in the suburbs.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 15000m,
                Area = 450,
                Bedrooms = 1,
                Bathrooms = 1,
                City = "Mumbai",
                Locality = "Kandivali East",
                Address = "Akurli Road, Kandivali East, Mumbai 400101",
                Furnishing = "Unfurnished",
                ImageUrl = "/assets/images/prop-18.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-1),
                CreatedAt = now.AddDays(-1)
            },
            new Property
            {
                Id = 19,
                Title = "Prestigious 2BHK Apartment in Bandra West",
                Description = "Sophisticated 2BHK apartment in the queen of suburbs - Bandra West. Close to Bandstand, Carter Road, and Linking Road. Premium tower with sea views, rooftop lounge, and state-of-the-art fitness center. Mount Mary Church nearby.",
                PropertyType = "Apartment",
                ListingType = "Buy",
                Price = 38000000m,
                Area = 1050,
                Bedrooms = 2,
                Bathrooms = 2,
                City = "Mumbai",
                Locality = "Bandra West",
                Address = "Hill Road, Bandra West, Mumbai 400050",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-19.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-11),
                CreatedAt = now.AddDays(-11)
            },
            new Property
            {
                Id = 20,
                Title = "Family 2BHK Flat in Borivali West",
                Description = "Comfortable 2BHK flat in Borivali West near Sanjay Gandhi National Park. Peaceful green surroundings with fresh air. Close to Borivali Railway Station and Western Express Highway. Family-friendly society with garden and play area.",
                PropertyType = "Apartment",
                ListingType = "Rent",
                Price = 22000m,
                Area = 800,
                Bedrooms = 2,
                Bathrooms = 1,
                City = "Mumbai",
                Locality = "Borivali West",
                Address = "IC Colony, Borivali West, Mumbai 400103",
                Furnishing = "Semi-Furnished",
                ImageUrl = "/assets/images/prop-20.jpg",
                IsAvailable = true,
                PostedDate = now.AddDays(-5),
                CreatedAt = now.AddDays(-5)
            }
        };
    }
}
