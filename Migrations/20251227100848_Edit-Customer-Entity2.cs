using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditCustomerEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_customer_FactoryId",
                table: "customer",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_factory_FactoryId",
                table: "customer",
                column: "FactoryId",
                principalTable: "factory",
                principalColumn: "FactoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_factory_FactoryId",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "IX_customer_FactoryId",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "customer");
        }
    }
}
