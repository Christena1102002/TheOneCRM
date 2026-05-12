using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerServices_Services_ServiceId",
                table: "customerServices");

            migrationBuilder.DropForeignKey(
                name: "FK_customerServices_customers_customerId",
                table: "customerServices");

            //migrationBuilder.DropIndex(
            //    name: "IX_DailyReports_UserId_ReportDate",
            //    table: "DailyReports");

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_UserId",
                table: "DailyReports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_customerServices_Services_ServiceId",
                table: "customerServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customerServices_customers_customerId",
                table: "customerServices",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerServices_Services_ServiceId",
                table: "customerServices");

            migrationBuilder.DropForeignKey(
                name: "FK_customerServices_customers_customerId",
                table: "customerServices");

            migrationBuilder.DropIndex(
                name: "IX_DailyReports_UserId",
                table: "DailyReports");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DailyReports_UserId_ReportDate",
            //    table: "DailyReports",
            //    columns: new[] { "UserId", "ReportDate" },
            //    unique: true,
            //    filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_customerServices_Services_ServiceId",
                table: "customerServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_customerServices_customers_customerId",
                table: "customerServices",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id");
        }
    }
}
