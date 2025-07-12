using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborhoodServices.Migrations
{
    /// <inheritdoc />
    public partial class InitNeighborhoodDbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Category", "Description", "Price", "ProviderId", "Title" },
                values: new object[] { 10, "Outdoor", "Trim, prune, and maintain your home garden", 65.00m, 1, "Gardening & Lawn Care" });
        }
    }
}
