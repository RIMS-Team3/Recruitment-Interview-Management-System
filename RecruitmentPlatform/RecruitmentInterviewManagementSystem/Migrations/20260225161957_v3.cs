using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentInterviewManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Interviews_IdInterviewSlot",
                table: "Interviews");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_IdInterviewSlot",
                table: "Interviews",
                column: "IdInterviewSlot",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Interviews_IdInterviewSlot",
                table: "Interviews");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_IdInterviewSlot",
                table: "Interviews",
                column: "IdInterviewSlot");
        }
    }
}
