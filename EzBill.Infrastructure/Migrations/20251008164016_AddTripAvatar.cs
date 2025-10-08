using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarTrip",
                table: "Trip",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarTrip",
                table: "Trip");
        }
    }
}
