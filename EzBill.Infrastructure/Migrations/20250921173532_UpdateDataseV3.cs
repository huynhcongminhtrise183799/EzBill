using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_TaxRefund_TaxRefundId",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_To_Account",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_Trip",
                table: "PaymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistory_TaxRefundId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "TaxRefundId",
                table: "PaymentHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "TripId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToAccountId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<long>(
                name: "OrderCode",
                table: "PaymentHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserDeviceToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PlanId",
                table: "PaymentHistory",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_RelatedTaxRefundId",
                table: "PaymentHistory",
                column: "RelatedTaxRefundId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeviceToken_AccountId",
                table: "UserDeviceToken",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_Plan",
                table: "PaymentHistory",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_TaxRefund",
                table: "PaymentHistory",
                column: "RelatedTaxRefundId",
                principalTable: "TaxRefund",
                principalColumn: "TaxRefundId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_To_Account",
                table: "PaymentHistory",
                column: "ToAccountId",
                principalTable: "Account",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_Trip",
                table: "PaymentHistory",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_Plan",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_TaxRefund",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_To_Account",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_Trip",
                table: "PaymentHistory");

            migrationBuilder.DropTable(
                name: "UserDeviceToken");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistory_PlanId",
                table: "PaymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistory_RelatedTaxRefundId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "PaymentHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "TripId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ToAccountId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaxRefundId",
                table: "PaymentHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_TaxRefundId",
                table: "PaymentHistory",
                column: "TaxRefundId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_TaxRefund_TaxRefundId",
                table: "PaymentHistory",
                column: "TaxRefundId",
                principalTable: "TaxRefund",
                principalColumn: "TaxRefundId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_To_Account",
                table: "PaymentHistory",
                column: "ToAccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_Trip",
                table: "PaymentHistory",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
