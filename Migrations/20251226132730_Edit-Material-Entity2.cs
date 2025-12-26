using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterialEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_materials_FactoryId",
                table: "materials",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_materials_factory_FactoryId",
                table: "materials",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materials_factory_FactoryId",
                table: "materials");

            migrationBuilder.DropIndex(
                name: "IX_materials_FactoryId",
                table: "materials");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "materials");
        }
    }
}
