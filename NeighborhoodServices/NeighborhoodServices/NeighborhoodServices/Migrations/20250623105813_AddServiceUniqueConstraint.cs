using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborhoodServices.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Services_Title_ProviderId",
                table: "Services",
                columns: new[] { "Title", "ProviderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Title_ProviderId",
                table: "Services");
        }
    }
}
