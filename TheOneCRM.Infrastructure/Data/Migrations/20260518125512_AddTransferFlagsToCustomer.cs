using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferFlagsToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsMarketingToSales",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSalesToSupport",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupportToSales",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarketingToSales",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "IsSalesToSupport",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "IsSupportToSales",
                table: "customers");

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
    }
}
