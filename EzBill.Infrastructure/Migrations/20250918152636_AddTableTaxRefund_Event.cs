using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTaxRefund_Event : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxRefund_Event",
                columns: table => new
                {
                    TaxRefundId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRefund_Event", x => new { x.TaxRefundId, x.EventId });
                    table.ForeignKey(
                        name: "FK_TaxRefund_Event_Event",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxRefund_Event_TaxRefund",
                        column: x => x.TaxRefundId,
                        principalTable: "TaxRefund",
                        principalColumn: "TaxRefundId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_Event_EventId",
                table: "TaxRefund_Event",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxRefund_Event");
        }
    }
}
