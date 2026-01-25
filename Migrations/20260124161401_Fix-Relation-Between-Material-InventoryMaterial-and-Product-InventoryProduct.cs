using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationBetweenMaterialInventoryMaterialandProductInventoryProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_materials_inventoryMaterials_InventoryId",
                table: "materials",
                column: "InventoryId",
                principalTable: "inventoryMaterials",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_inventoryProducts_InventoryId",
                table: "products",
                column: "InventoryId",
                principalTable: "inventoryProducts",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryMaterials_materials_MaterialId",
                table: "inventoryMaterials",
                column: "MaterialId",
                principalTable: "materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryProducts_products_ProductId",
                table: "inventoryProducts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
