using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NeighborhoodServices.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Category", "Description", "Price", "ProviderId", "Title" },
                values: new object[,]
                {
                    { 3, "Fitness", "Join our guided yoga sessions for better health", 35.00m, 1, "Yoga Classes" },
                    { 4, "Home Services", "Thorough home cleaning with eco-friendly supplies", 50.00m, 1, "House Cleaning" },
                    { 5, "Beauty", "Professional grooming service at your home", 30.00m, 1, "Haircut & Grooming" },
                    { 6, "Maintenance", "Fix leaks and broken pipes with our expert plumber", 60.00m, 1, "Plumbing Repairs" },
                    { 7, "Tech Support", "Get your phone fixed at home by a technician", 45.00m, 1, "Mobile Phone Repair" },
                    { 8, "Education", "One-on-one beginner piano instruction", 55.00m, 1, "Piano Lessons" },
                    { 9, "Home Services", "Pickup and delivery laundry wash & fold", 20.00m, 1, "Laundry Service" },
                    { 10, "Outdoor", "Trim, prune, and maintain your home garden", 65.00m, 1, "Gardening & Lawn Care" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
