using Microsoft.EntityFrameworkCore.Migrations;

namespace Auxeltus.AccessLayer.Sql.Migrations
{
    public partial class FixJobRemoteCheckConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Jobs");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Jobs",
                sql: "([LocationId] IS NOT NULL AND [Remote] = 0) OR ([Remote] = 1 AND [LocationId] IS NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Jobs");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EmployeeLocationSanity",
                table: "Jobs",
                sql: "[LocationId] IS NOT NULL OR [Remote] = 1");
        }
    }
}
