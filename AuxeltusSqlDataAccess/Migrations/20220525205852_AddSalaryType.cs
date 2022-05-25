using Microsoft.EntityFrameworkCore.Migrations;

namespace Auxeltus.AccessLayer.Sql.Migrations
{
    public partial class AddSalaryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalaryType",
                table: "Jobs",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryType",
                table: "Jobs");
        }
    }
}
