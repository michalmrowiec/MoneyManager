using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Server.Migrations
{
    public partial class CategoryNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "CategoryName");
        }
    }
}
