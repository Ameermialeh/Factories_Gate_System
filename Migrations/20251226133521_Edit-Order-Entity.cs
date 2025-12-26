using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDescription",
                table: "orders");
            migrationBuilder.Sql(
               "ALTER TABLE `orders` CHANGE `OrderName` `Name` VARCHAR(255);"
           );
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orders_FactoryId",
                table: "orders",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_factory_FactoryId",
                table: "orders",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_factory_FactoryId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_FactoryId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "OrderDescription",
                table: "orders");

            migrationBuilder.Sql(
               "ALTER TABLE `orders` CHANGE `OrderName` `Name` VARCHAR(255);"
           );

            migrationBuilder.AddColumn<string>(
                name: "FactoryId",
                table: "orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
