using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditManagerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE `manager` CHANGE `ManagerName` `Name` VARCHAR(255);"
            );
            migrationBuilder.Sql(
               "ALTER TABLE `manager` CHANGE `ManagerEmail` `Email` VARCHAR(255);"
           );

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "manager",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "manager");

            migrationBuilder.Sql(
                "ALTER TABLE `manager` CHANGE `ManagerName` `Name` VARCHAR(255);"
            );
            migrationBuilder.Sql(
               "ALTER TABLE `manager` CHANGE `ManagerEmail` `Email` VARCHAR(255);"
           );
        }
    }
}
