using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferFlagsToAssignmentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarketingToSales",
                table: "CustomerAssignmentHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSalesToSupport",
                table: "CustomerAssignmentHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupportToSales",
                table: "CustomerAssignmentHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarketingToSales",
                table: "CustomerAssignmentHistories");

            migrationBuilder.DropColumn(
                name: "IsSalesToSupport",
                table: "CustomerAssignmentHistories");

            migrationBuilder.DropColumn(
                name: "IsSupportToSales",
                table: "CustomerAssignmentHistories");
        }
    }
}
