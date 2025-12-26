using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "customer");

            migrationBuilder.Sql(
                "ALTER TABLE `customer` CHANGE `PhoneNumber` `Phone` VARCHAR(255);"
            );
            migrationBuilder.Sql(
                "ALTER TABLE `customer` CHANGE `CustomerName` `Name` VARCHAR(255);"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE `customer` CHANGE `PhoneNumber` `Phone` VARCHAR(255);"
            );
            migrationBuilder.Sql(
                "ALTER TABLE `customer` CHANGE `CustomerName` `Name` VARCHAR(255);"
            );

            migrationBuilder.AddColumn<int>(
                name: "CurrentBalance",
                table: "customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
