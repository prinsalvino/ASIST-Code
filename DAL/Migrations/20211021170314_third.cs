using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "TestAttempted",
                columns: table => new
                {
                    TestAttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalScore = table.Column<int>(type: "int", nullable: false),
                    DateOfTest = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempted", x => x.TestAttemptId);
                });

            migrationBuilder.CreateTable(
                name: "StudentTestAttempted",
                columns: table => new
                {
                    StudentsUserId = table.Column<long>(type: "bigint", nullable: false),
                    TestsAttemptedTestAttemptId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestAttempted", x => new { x.StudentsUserId, x.TestsAttemptedTestAttemptId });
                    table.ForeignKey(
                        name: "FK_StudentTestAttempted_TestAttempted_TestsAttemptedTestAttemptId",
                        column: x => x.TestsAttemptedTestAttemptId,
                        principalTable: "TestAttempted",
                        principalColumn: "TestAttemptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTestAttempted_Users_StudentsUserId",
                        column: x => x.StudentsUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAttempted_TestsAttemptedTestAttemptId",
                table: "StudentTestAttempted",
                column: "TestsAttemptedTestAttemptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTestAttempted");

            migrationBuilder.DropTable(
                name: "TestAttempted");

            migrationBuilder.AddColumn<float>(
                name: "FinalScore",
                table: "Users",
                type: "real",
                nullable: true);
        }
    }
}
