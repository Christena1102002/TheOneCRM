using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedReportDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(type: "date", nullable: false),
                    CompletedTasks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    TasksInProgress = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PlannedTasks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Challenges = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    WorkHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_ReportDate",
                table: "DailyReports",
                column: "ReportDate");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DailyReports_UserId_ReportDate",
            //    table: "DailyReports",
            //    columns: new[] { "UserId", "ReportDate" },
            //    unique: true,
            //    filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyReports");
        }
    }
}
