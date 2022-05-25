using Microsoft.EntityFrameworkCore.Migrations;

namespace Auxeltus.AccessLayer.Sql.Migrations
{
    public partial class JobRelatedUniquenessConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Roles_Title",
                table: "Roles",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Latitude_Longitude",
                table: "Locations",
                columns: new[] { "Latitude", "Longitude" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Title",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Latitude_Longitude",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Name",
                table: "Locations");
        }
    }
}
