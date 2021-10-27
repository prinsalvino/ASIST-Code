using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTestAttempted");

            migrationBuilder.AddColumn<long>(
                name: "StudentUserId",
                table: "TestAttempted",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempted_StudentUserId",
                table: "TestAttempted",
                column: "StudentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttempted_Users_StudentUserId",
                table: "TestAttempted",
                column: "StudentUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestAttempted_Users_StudentUserId",
                table: "TestAttempted");

            migrationBuilder.DropIndex(
                name: "IX_TestAttempted_StudentUserId",
                table: "TestAttempted");

            migrationBuilder.DropColumn(
                name: "StudentUserId",
                table: "TestAttempted");

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
    }
}
