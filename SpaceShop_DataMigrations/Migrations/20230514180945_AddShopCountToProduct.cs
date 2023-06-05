using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop_DataMigrations
{
    public partial class AddShopCountToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopCount",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopCount",
                table: "Product");
        }
    }
}
