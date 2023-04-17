using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop_DataMigrations
{
    public partial class RemoveColumnsFromQueryHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExecution",
                table: "QueryHeader");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "QueryHeader");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExecution",
                table: "QueryHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "QueryHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
