using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborhoodServices.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueBookingConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServiceBookings_ServiceId",
                table: "ServiceBookings");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_ServiceId_CustomerId_ScheduledTime",
                table: "ServiceBookings",
                columns: new[] { "ServiceId", "CustomerId", "ScheduledTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServiceBookings_ServiceId_CustomerId_ScheduledTime",
                table: "ServiceBookings");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_ServiceId",
                table: "ServiceBookings",
                column: "ServiceId");
        }
    }
}
