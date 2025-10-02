using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBill.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnCreateAtInSettlementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Settlement",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreateAt",
                table: "Settlement",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
