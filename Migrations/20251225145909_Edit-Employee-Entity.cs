using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditEmployeeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeSalary",
                table: "employees");

            migrationBuilder.Sql(
                "ALTER TABLE `employees` CHANGE `EmployeeName` `Name` VARCHAR(255);"
            );

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "employees",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "employees");
            migrationBuilder.Sql(
                "ALTER TABLE `employees` CHANGE `EmployeeName` `Name` VARCHAR(255);"
            );
            migrationBuilder.AddColumn<int>(
                name: "EmployeeSalary",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
