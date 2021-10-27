using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportStudent_Users_StudentUserId",
                table: "SportStudent");

            migrationBuilder.DropIndex(
                name: "IX_SportStudent_StudentUserId",
                table: "SportStudent");

            migrationBuilder.DropColumn(
                name: "StudentUserId",
                table: "SportStudent");

            migrationBuilder.AddColumn<long>(
                name: "StudentId",
                table: "SportStudent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SportStudent_StudentId",
                table: "SportStudent",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SportStudent_Users_StudentId",
                table: "SportStudent",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportStudent_Users_StudentId",
                table: "SportStudent");

            migrationBuilder.DropIndex(
                name: "IX_SportStudent_StudentId",
                table: "SportStudent");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "SportStudent");

            migrationBuilder.AddColumn<long>(
                name: "StudentUserId",
                table: "SportStudent",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SportStudent_StudentUserId",
                table: "SportStudent",
                column: "StudentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SportStudent_Users_StudentUserId",
                table: "SportStudent",
                column: "StudentUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
