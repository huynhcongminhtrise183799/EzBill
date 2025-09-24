using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Plan",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "Account",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "Account");
        }
    }
}
