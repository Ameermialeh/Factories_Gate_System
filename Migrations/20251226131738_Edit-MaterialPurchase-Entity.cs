using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterialPurchaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE `MaterialPurchase` CHANGE `Price` `PricePerUnit` VARCHAR(255);"
            );
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
    "ALTER TABLE `MaterialPurchase` CHANGE `Price` `PricePerUnit` VARCHAR(255);"
);
        }
    }
}
