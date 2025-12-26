using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hide",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "products");

            migrationBuilder.Sql(
               "ALTER TABLE `products` CHANGE `ProductQuantity` `StockQuantity` VARCHAR(255);"
           );
            migrationBuilder.Sql(
                "ALTER TABLE `products` CHANGE `ProductPrice` `Price` VARCHAR(255);"
            );

            migrationBuilder.Sql(
               "ALTER TABLE `products` CHANGE `ProductName` `Name` VARCHAR(255);"
           );
            

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_products_FactoryId",
                table: "products",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_factory_FactoryId",
                table: "products",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_factory_FactoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_FactoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "products");
            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "products");

            migrationBuilder.Sql(
               "ALTER TABLE `products` CHANGE `ProductQuantity` `StockQuantity` VARCHAR(255);"
           );
            migrationBuilder.Sql(
                "ALTER TABLE `products` CHANGE `ProductPrice` `Price` VARCHAR(255);"
            );

            migrationBuilder.Sql(
               "ALTER TABLE `products` CHANGE `ProductName` `Name` VARCHAR(255);"
           );

            migrationBuilder.AddColumn<bool>(
                name: "FactoryId",
                table: "products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
