using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BaseProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripName = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Budget = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_TRIP_ACCOUNT",
                        column: x => x.TripId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    EventDescription = table.Column<string>(type: "text", nullable: false),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaidBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<char>(type: "character(1)", nullable: false),
                    AmountOriginal = table.Column<double>(type: "double precision", nullable: false),
                    ExchangeRate = table.Column<double>(type: "double precision", nullable: true),
                    AmountInTripCurrency = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EVENT_ACCOUNT",
                        column: x => x.PaidBy,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TRIP_EVENT",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settlement",
                columns: table => new
                {
                    SettlementId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement", x => x.SettlementId);
                    table.ForeignKey(
                        name: "FK_Settlement_From_Account",
                        column: x => x.FromAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Settlement_To_Account",
                        column: x => x.ToAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Settlement_Trip",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripMember",
                columns: table => new
                {
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripMember", x => new { x.TripId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_TRIPMEMBER_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TRIPMEMBER_TRIP",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Use",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Use", x => new { x.AccountId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EVENT_USE_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVENT_USE_EVENT",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxRefund",
                columns: table => new
                {
                    TaxRefundId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    OriginalAmount = table.Column<double>(type: "double precision", nullable: false),
                    RefundPercent = table.Column<double>(type: "double precision", nullable: false),
                    RefundAmount = table.Column<double>(type: "double precision", nullable: false),
                    RefundedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsGroupMoneyUsed = table.Column<bool>(type: "boolean", nullable: false),
                    SplitType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRefund", x => x.TaxRefundId);
                    table.ForeignKey(
                        name: "FK_TaxRefund_Account",
                        column: x => x.RefundedBy,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxRefund_Event",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentHistory",
                columns: table => new
                {
                    PaymentHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    PaymentUrlBill = table.Column<string>(type: "text", nullable: true),
                    RelatedTaxRefundId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentType = table.Column<string>(type: "text", nullable: false),
                    TaxRefundId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistory", x => x.PaymentHistoryId);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_From_Account",
                        column: x => x.FromAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_TaxRefund_TaxRefundId",
                        column: x => x.TaxRefundId,
                        principalTable: "TaxRefund",
                        principalColumn: "TaxRefundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_To_Account",
                        column: x => x.ToAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Trip",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxRefund_Usage",
                columns: table => new
                {
                    TaxRefundId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ratio = table.Column<double>(type: "double precision", nullable: true),
                    AmountReceived = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRefund_Usage", x => new { x.AccountId, x.TaxRefundId });
                    table.ForeignKey(
                        name: "FK_TaxRefund_Usage_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxRefund_Usage_TaxRefund",
                        column: x => x.TaxRefundId,
                        principalTable: "TaxRefund",
                        principalColumn: "TaxRefundId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_PaidBy",
                table: "Event",
                column: "PaidBy");

            migrationBuilder.CreateIndex(
                name: "IX_Event_TripId",
                table: "Event",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Use_EventId",
                table: "Event_Use",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_FromAccountId",
                table: "PaymentHistory",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_TaxRefundId",
                table: "PaymentHistory",
                column: "TaxRefundId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_ToAccountId",
                table: "PaymentHistory",
                column: "ToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_TripId",
                table: "PaymentHistory",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_FromAccountId",
                table: "Settlement",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_ToAccountId",
                table: "Settlement",
                column: "ToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_TripId",
                table: "Settlement",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_EventId",
                table: "TaxRefund",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_RefundedBy",
                table: "TaxRefund",
                column: "RefundedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_Usage_TaxRefundId",
                table: "TaxRefund_Usage",
                column: "TaxRefundId");

            migrationBuilder.CreateIndex(
                name: "IX_TripMember_AccountId",
                table: "TripMember",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Use");

            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DropTable(
                name: "Settlement");

            migrationBuilder.DropTable(
                name: "TaxRefund_Usage");

            migrationBuilder.DropTable(
                name: "TripMember");

            migrationBuilder.DropTable(
                name: "TaxRefund");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Account");
        }
    }
}
