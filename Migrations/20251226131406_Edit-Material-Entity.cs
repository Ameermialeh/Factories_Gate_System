using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterialEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hide",
                table: "materials");

            migrationBuilder.Sql(
                "ALTER TABLE `materials` CHANGE `MaterialName` `Name` VARCHAR(255);"
            );

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "materials",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hide",
                table: "materials");
            migrationBuilder.Sql(
                "ALTER TABLE `materials` CHANGE `MaterialName` `Name` VARCHAR(255);"
            );
           
            migrationBuilder.AddColumn<bool>(
                name: "Unit",
                table: "materials",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
