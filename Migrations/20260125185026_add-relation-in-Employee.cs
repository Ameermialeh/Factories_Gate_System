using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class addrelationinEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_employees_FactoryId",
                table: "employees",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_factory_FactoryId",
                table: "employees",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_factory_FactoryId",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_FactoryId",
                table: "employees");
        }
    }
}
