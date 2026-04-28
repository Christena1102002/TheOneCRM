using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intialdb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deals_AspNetUsers_CreatedById",
                table: "deals");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "deals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_deals_AssignedToId",
                table: "deals",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_deals_AspNetUsers_AssignedToId",
                table: "deals",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_deals_AspNetUsers_CreatedById",
                table: "deals",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deals_AspNetUsers_AssignedToId",
                table: "deals");

            migrationBuilder.DropForeignKey(
                name: "FK_deals_AspNetUsers_CreatedById",
                table: "deals");

            migrationBuilder.DropIndex(
                name: "IX_deals_AssignedToId",
                table: "deals");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "deals");

            migrationBuilder.AddForeignKey(
                name: "FK_deals_AspNetUsers_CreatedById",
                table: "deals",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
