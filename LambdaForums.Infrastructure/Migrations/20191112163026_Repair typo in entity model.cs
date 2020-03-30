using Microsoft.EntityFrameworkCore.Migrations;

namespace LambdaForums.Infrastructure.Migrations
{
    public partial class Repairtypoinentitymodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DEscription",
                table: "Forums",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Forums",
                newName: "DEscription");
        }
    }
}
