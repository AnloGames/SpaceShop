using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop_DataMigrations
{
    public partial class RemoveQueryAndChangeOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_AspNetUsers_AdminId",
                table: "OrderHeader");

            migrationBuilder.DropTable(
                name: "QueryDetail");

            migrationBuilder.DropTable(
                name: "QueryHeader");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "OrderHeader",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeader_AdminId",
                table: "OrderHeader",
                newName: "IX_OrderHeader_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_AspNetUsers_UserId",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OrderHeader",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeader_UserId",
                table: "OrderHeader",
                newName: "IX_OrderHeader_AdminId");

            migrationBuilder.CreateTable(
                name: "QueryHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    QueryDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryHeader_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueryDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    QueryHeaderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueryDetail_QueryHeader_QueryHeaderId",
                        column: x => x.QueryHeaderId,
                        principalTable: "QueryHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueryDetail_ProductId",
                table: "QueryDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryDetail_QueryHeaderId",
                table: "QueryDetail",
                column: "QueryHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryHeader_ApplicationUserId",
                table: "QueryHeader",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_AspNetUsers_AdminId",
                table: "OrderHeader",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
