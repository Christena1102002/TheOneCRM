using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AspNetUsers_AssignedToId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AssignedToId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "Contracts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AssignedToId",
                table: "Contracts",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_AspNetUsers_AssignedToId",
                table: "Contracts",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
