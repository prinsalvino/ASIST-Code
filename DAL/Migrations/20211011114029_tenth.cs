using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class tenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TimeOfCompletion",
                table: "SkillStudent",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfSkill",
                table: "SkillStudent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfSkill",
                table: "SkillStudent");

            migrationBuilder.AlterColumn<float>(
                name: "TimeOfCompletion",
                table: "SkillStudent",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
