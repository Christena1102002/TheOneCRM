using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedProbertiesToCampains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CampaignCountry");

            migrationBuilder.AddColumn<decimal>(
                name: "DailyBudget",
                table: "campaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "campaigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyBudget",
                table: "campaigns");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "campaigns");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "campaigns");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CampaignCountry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
