using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Sports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Sports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
