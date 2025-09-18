using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseWithEvent_Use : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_ACCOUNT",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Event_Use",
                newName: "AmountFromPersonal");

            migrationBuilder.AddColumn<double>(
                name: "AmountRemainInTrip",
                table: "TripMember",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountFromGroup",
                table: "Event_Use",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PaidBy",
                table: "Event",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "GroupRemaining",
                table: "AccountSubscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_ACCOUNT",
                table: "Event",
                column: "PaidBy",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_ACCOUNT",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "AmountRemainInTrip",
                table: "TripMember");

            migrationBuilder.DropColumn(
                name: "AmountFromGroup",
                table: "Event_Use");

            migrationBuilder.DropColumn(
                name: "GroupRemaining",
                table: "AccountSubscriptions");

            migrationBuilder.RenameColumn(
                name: "AmountFromPersonal",
                table: "Event_Use",
                newName: "Amount");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaidBy",
                table: "Event",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_ACCOUNT",
                table: "Event",
                column: "PaidBy",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
