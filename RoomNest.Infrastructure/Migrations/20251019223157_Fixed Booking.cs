using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_HotelId_RoomId_CheckInDate_CheckOutDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelId_RoomId_CheckInDate_CheckOutDate",
                table: "Bookings",
                columns: new[] { "HotelId", "RoomId", "CheckInDate", "CheckOutDate" });
        }
    }
}
