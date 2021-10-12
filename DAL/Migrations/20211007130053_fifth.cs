using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_CoachUserId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_StudentUserId",
                table: "SkillStudent");

            migrationBuilder.RenameColumn(
                name: "StudentUserId",
                table: "SkillStudent",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "CoachUserId",
                table: "SkillStudent",
                newName: "CoachId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_StudentUserId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_CoachUserId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_StudentId",
                table: "SkillStudent",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_StudentId",
                table: "SkillStudent");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "SkillStudent",
                newName: "StudentUserId");

            migrationBuilder.RenameColumn(
                name: "CoachId",
                table: "SkillStudent",
                newName: "CoachUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_StudentId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_StudentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_CoachId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_CoachUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_CoachUserId",
                table: "SkillStudent",
                column: "CoachUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_StudentUserId",
                table: "SkillStudent",
                column: "StudentUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
