using Microsoft.EntityFrameworkCore.Migrations;

namespace Auxeltus.AccessLayer.Sql.Migrations
{
    public partial class JobContraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Location_LocationId",
                table: "Job");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Job",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeType",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Remote",
                table: "Job",
                type: "bit",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_EmployeeHasSalary",
                table: "Job",
                sql: "([EmployeeId] IS NULL AND [Salary] IS NULL) OR ([EmployeeId] IS NOT NULL AND [Salary] IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Job",
                sql: "[LocationId] IS NOT NULL OR [Remote] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Location_LocationId",
                table: "Job",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Location_LocationId",
                table: "Job");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmployeeHasSalary",
                table: "Job");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Remote",
                table: "Job");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Location_LocationId",
                table: "Job",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
