using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxRefund_Event",
                table: "TaxRefund");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "TaxRefund",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_TaxRefund_EventId",
                table: "TaxRefund",
                newName: "IX_TaxRefund_TripId");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Event_Use",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SplitType",
                table: "Event",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Account",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxRefund_Trip",
                table: "TaxRefund",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxRefund_Trip",
                table: "TaxRefund");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Event_Use");

            migrationBuilder.DropColumn(
                name: "SplitType",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "TaxRefund",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_TaxRefund_TripId",
                table: "TaxRefund",
                newName: "IX_TaxRefund_EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxRefund_Event",
                table: "TaxRefund",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
