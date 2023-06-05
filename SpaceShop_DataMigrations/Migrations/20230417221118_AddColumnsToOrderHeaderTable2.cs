using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop_DataMigrations
{
    public partial class AddColumnsToOrderHeaderTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExecution",
                table: "OrderHeader",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OrderHeader",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExecution",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OrderHeader");
        }
    }
}
