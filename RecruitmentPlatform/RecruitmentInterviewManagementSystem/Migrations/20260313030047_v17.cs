using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentInterviewManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class v17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_CandidateId",
                table: "Orders",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CandidateProfiles_CandidateId",
                table: "Orders",
                column: "CandidateId",
                principalTable: "CandidateProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CandidateProfiles_CandidateId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CandidateId",
                table: "Orders");
        }
    }
}
