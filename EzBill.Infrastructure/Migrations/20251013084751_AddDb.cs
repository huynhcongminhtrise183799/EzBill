using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    NickName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<bool>(type: "boolean", nullable: false),
                    QrCodeUrl = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    MaxMembersPerTrip = table.Column<int>(type: "integer", nullable: false),
                    MaxGroups = table.Column<int>(type: "integer", nullable: false),
                    BillingCycle = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "ForgotPassword",
                columns: table => new
                {
                    ForgotPasswordId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    OTP = table.Column<string>(type: "text", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPassword", x => x.ForgotPasswordId);
                    table.ForeignKey(
                        name: "FK_ForgotPassword_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripName = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AvatarTrip = table.Column<string>(type: "text", nullable: true),
                    Budget = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_TRIP_ACCOUNT",
                        column: x => x.CreatedBy,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDeviceToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FCMToken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeviceToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDeviceToken_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountSubscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    GroupRemaining = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSubscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_AccountSubscriptions_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountSubscriptions_Plan",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    EventDescription = table.Column<string>(type: "text", nullable: false),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaidBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    AmountOriginal = table.Column<double>(type: "double precision", nullable: false),
                    ExchangeRate = table.Column<double>(type: "double precision", nullable: true),
                    AmountInTripCurrency = table.Column<double>(type: "double precision", nullable: false),
                    SplitType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EVENT_ACCOUNT",
                        column: x => x.PaidBy,
                        principalTable: "Account",
                        principalColumn: "AccountId");
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
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
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
                name: "TaxRefund",
                columns: table => new
                {
                    TaxRefundId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_TaxRefund_Trip",
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
                    AmountRemainInTrip = table.Column<double>(type: "double precision", nullable: true),
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
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountFromGroup = table.Column<double>(type: "double precision", nullable: true),
                    AmountFromPersonal = table.Column<double>(type: "double precision", nullable: true)
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
                name: "PaymentHistory",
                columns: table => new
                {
                    PaymentHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderCode = table.Column<long>(type: "bigint", nullable: true),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    PaymentUrlBill = table.Column<string>(type: "text", nullable: true),
                    RelatedTaxRefundId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentType = table.Column<string>(type: "text", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
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
                        name: "FK_PaymentHistory_Plan",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "PlanId");
                    table.ForeignKey(
                        name: "FK_PaymentHistory_TaxRefund",
                        column: x => x.RelatedTaxRefundId,
                        principalTable: "TaxRefund",
                        principalColumn: "TaxRefundId");
                    table.ForeignKey(
                        name: "FK_PaymentHistory_To_Account",
                        column: x => x.ToAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Trip",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId");
                });

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
                name: "IX_AccountSubscriptions_AccountId",
                table: "AccountSubscriptions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_PlanId",
                table: "AccountSubscriptions",
                column: "PlanId");

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
                name: "IX_ForgotPassword_AccountId",
                table: "ForgotPassword",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_FromAccountId",
                table: "PaymentHistory",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PlanId",
                table: "PaymentHistory",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_RelatedTaxRefundId",
                table: "PaymentHistory",
                column: "RelatedTaxRefundId");

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
                name: "IX_TaxRefund_RefundedBy",
                table: "TaxRefund",
                column: "RefundedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_TripId",
                table: "TaxRefund",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_Event_EventId",
                table: "TaxRefund_Event",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRefund_Usage_TaxRefundId",
                table: "TaxRefund_Usage",
                column: "TaxRefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_CreatedBy",
                table: "Trip",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TripMember_AccountId",
                table: "TripMember",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeviceToken_AccountId",
                table: "UserDeviceToken",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSubscriptions");

            migrationBuilder.DropTable(
                name: "Event_Use");

            migrationBuilder.DropTable(
                name: "ForgotPassword");

            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DropTable(
                name: "Settlement");

            migrationBuilder.DropTable(
                name: "TaxRefund_Event");

            migrationBuilder.DropTable(
                name: "TaxRefund_Usage");

            migrationBuilder.DropTable(
                name: "TripMember");

            migrationBuilder.DropTable(
                name: "UserDeviceToken");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "TaxRefund");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
