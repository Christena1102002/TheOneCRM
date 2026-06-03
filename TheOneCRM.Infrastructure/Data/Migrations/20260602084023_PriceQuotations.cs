using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PriceQuotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "PriceQuotations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_CreatedById",
                table: "PriceQuotations",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                table: "PriceQuotations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                table: "PriceQuotations");

            migrationBuilder.DropIndex(
                name: "IX_PriceQuotations_CreatedById",
                table: "PriceQuotations");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PriceQuotations");
        }
    }
}
