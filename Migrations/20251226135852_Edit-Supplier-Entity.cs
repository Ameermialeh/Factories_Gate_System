using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditSupplierEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_suppliers_FactoryId",
                table: "suppliers",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_suppliers_factory_FactoryId",
                table: "suppliers",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_suppliers_factory_FactoryId",
                table: "suppliers");

            migrationBuilder.DropIndex(
                name: "IX_suppliers_FactoryId",
                table: "suppliers");

            migrationBuilder.AddColumn<bool>(
                name: "FactoryId",
                table: "suppliers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
