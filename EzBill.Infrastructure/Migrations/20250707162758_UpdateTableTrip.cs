using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRIP_ACCOUNT",
                table: "Trip");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_CreatedBy",
                table: "Trip",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TRIP_ACCOUNT",
                table: "Trip",
                column: "CreatedBy",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRIP_ACCOUNT",
                table: "Trip");

            migrationBuilder.DropIndex(
                name: "IX_Trip_CreatedBy",
                table: "Trip");

            migrationBuilder.AddForeignKey(
                name: "FK_TRIP_ACCOUNT",
                table: "Trip",
                column: "TripId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
