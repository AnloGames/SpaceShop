using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop.Migrations
{
    public partial class AddConnectionProductMyModelToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionProductMyModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MyModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionProductMyModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionProductMyModel_MyModel_MyModelId",
                        column: x => x.MyModelId,
                        principalTable: "MyModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionProductMyModel_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionProductMyModel_MyModelId",
                table: "ConnectionProductMyModel",
                column: "MyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionProductMyModel_ProductId",
                table: "ConnectionProductMyModel",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionProductMyModel");
        }
    }
}
