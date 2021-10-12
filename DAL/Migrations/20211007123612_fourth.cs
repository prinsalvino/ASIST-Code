using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSkills_Skills_SkillId",
                table: "StudentSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSkills_Users_CoachUserId",
                table: "StudentSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSkills_Users_StudentUserId",
                table: "StudentSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSkills",
                table: "StudentSkills");

            migrationBuilder.RenameTable(
                name: "StudentSkills",
                newName: "SkillStudent");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSkills_StudentUserId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_StudentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSkills_SkillId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSkills_CoachUserId",
                table: "SkillStudent",
                newName: "IX_SkillStudent_CoachUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillStudent",
                table: "SkillStudent",
                column: "SkillStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_CoachUserId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_StudentUserId",
                table: "SkillStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillStudent",
                table: "SkillStudent");

            migrationBuilder.RenameTable(
                name: "SkillStudent",
                newName: "StudentSkills");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_StudentUserId",
                table: "StudentSkills",
                newName: "IX_StudentSkills_StudentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_SkillId",
                table: "StudentSkills",
                newName: "IX_StudentSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_SkillStudent_CoachUserId",
                table: "StudentSkills",
                newName: "IX_StudentSkills_CoachUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSkills",
                table: "StudentSkills",
                column: "SkillStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSkills_Skills_SkillId",
                table: "StudentSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSkills_Users_CoachUserId",
                table: "StudentSkills",
                column: "CoachUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSkills_Users_StudentUserId",
                table: "StudentSkills",
                column: "StudentUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
