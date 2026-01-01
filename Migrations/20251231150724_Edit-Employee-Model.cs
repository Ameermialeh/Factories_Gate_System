using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "vacations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_vacations_FactoryId",
                table: "vacations",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_vacations_employees_FactoryId",
                table: "vacations",
                column: "FactoryId",
                principalTable: "employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vacations_employees_FactoryId",
                table: "vacations");

            migrationBuilder.DropIndex(
                name: "IX_vacations_FactoryId",
                table: "vacations");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "vacations");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "employees");
        }
    }
}
