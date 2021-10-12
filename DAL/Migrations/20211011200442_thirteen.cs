using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class thirteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_Users_CoachUserId",
                table: "Organisation");

            migrationBuilder.RenameColumn(
                name: "CoachUserId",
                table: "Organisation",
                newName: "CoachId");

            migrationBuilder.RenameIndex(
                name: "IX_Organisation_CoachUserId",
                table: "Organisation",
                newName: "IX_Organisation_CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_Users_CoachId",
                table: "Organisation",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_Users_CoachId",
                table: "Organisation");

            migrationBuilder.RenameColumn(
                name: "CoachId",
                table: "Organisation",
                newName: "CoachUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Organisation_CoachId",
                table: "Organisation",
                newName: "IX_Organisation_CoachUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_Users_CoachUserId",
                table: "Organisation",
                column: "CoachUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
