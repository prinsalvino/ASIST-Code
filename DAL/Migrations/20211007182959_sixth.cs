using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent");

            migrationBuilder.AlterColumn<long>(
                name: "SkillId",
                table: "SkillStudent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CoachId",
                table: "SkillStudent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent");

            migrationBuilder.AlterColumn<long>(
                name: "SkillId",
                table: "SkillStudent",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CoachId",
                table: "SkillStudent",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Skills_SkillId",
                table: "SkillStudent",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillStudent_Users_CoachId",
                table: "SkillStudent",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
