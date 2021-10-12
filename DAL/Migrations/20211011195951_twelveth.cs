using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class twelveth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachOrganisation");

            migrationBuilder.AddColumn<long>(
                name: "CoachUserId",
                table: "Organisation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_CoachUserId",
                table: "Organisation",
                column: "CoachUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_Users_CoachUserId",
                table: "Organisation",
                column: "CoachUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_Users_CoachUserId",
                table: "Organisation");

            migrationBuilder.DropIndex(
                name: "IX_Organisation_CoachUserId",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "CoachUserId",
                table: "Organisation");

            migrationBuilder.CreateTable(
                name: "CoachOrganisation",
                columns: table => new
                {
                    CoachesUserId = table.Column<long>(type: "bigint", nullable: false),
                    OrganisationsOrganisationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachOrganisation", x => new { x.CoachesUserId, x.OrganisationsOrganisationId });
                    table.ForeignKey(
                        name: "FK_CoachOrganisation_Organisation_OrganisationsOrganisationId",
                        column: x => x.OrganisationsOrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachOrganisation_Users_CoachesUserId",
                        column: x => x.CoachesUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachOrganisation_OrganisationsOrganisationId",
                table: "CoachOrganisation",
                column: "OrganisationsOrganisationId");
        }
    }
}
