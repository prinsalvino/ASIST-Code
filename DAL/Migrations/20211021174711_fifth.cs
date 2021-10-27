using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestAttempted");

            migrationBuilder.CreateTable(
                name: "TestAttempt",
                columns: table => new
                {
                    TestAttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    FinalScore = table.Column<int>(type: "int", nullable: false),
                    DateOfTest = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempt", x => x.TestAttemptId);
                    table.ForeignKey(
                        name: "FK_TestAttempt_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_StudentId",
                table: "TestAttempt",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestAttempt");

            migrationBuilder.CreateTable(
                name: "TestAttempted",
                columns: table => new
                {
                    TestAttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfTest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalScore = table.Column<int>(type: "int", nullable: false),
                    StudentUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempted", x => x.TestAttemptId);
                    table.ForeignKey(
                        name: "FK_TestAttempted_Users_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempted_StudentUserId",
                table: "TestAttempted",
                column: "StudentUserId");
        }
    }
}
