using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoriesGateSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditVacationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               "ALTER TABLE `vacations` CHANGE `VacationDate` `FromDate` VARCHAR(255);"
           );


            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "vacations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "vacations");

            migrationBuilder.Sql(
               "ALTER TABLE `vacations` CHANGE `VacationDate` `FromDate` VARCHAR(255);"
           );

        }
    }
}
