using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentInterviewManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterviewSlots",
                columns: table => new
                {
                    IdInterviewSlot = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSlots", x => x.IdInterviewSlot);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    IdInterview = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnicalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoftSkillScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdInterviewSlot = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdApplication = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.IdInterview);
                    table.ForeignKey(
                        name: "FK_Interviews_Applications_IdApplication",
                        column: x => x.IdApplication,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interviews_InterviewSlots_IdInterviewSlot",
                        column: x => x.IdInterviewSlot,
                        principalTable: "InterviewSlots",
                        principalColumn: "IdInterviewSlot",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_IdApplication",
                table: "Interviews",
                column: "IdApplication");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_IdInterviewSlot",
                table: "Interviews",
                column: "IdInterviewSlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropTable(
                name: "InterviewSlots");
        }
    }
}
