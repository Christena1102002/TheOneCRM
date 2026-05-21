using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SupportTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supportTickets_AspNetUsers_AssignedToId",
                table: "supportTickets");

            migrationBuilder.DropIndex(
                name: "IX_supportTickets_AssignedToId",
                table: "supportTickets");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "supportTickets");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "supportTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "supportTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_supportTickets_CustomerId",
                table: "supportTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_supportTickets_ServiceId",
                table: "supportTickets",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_supportTickets_Services_ServiceId",
                table: "supportTickets",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_supportTickets_customers_CustomerId",
                table: "supportTickets",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supportTickets_Services_ServiceId",
                table: "supportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_supportTickets_customers_CustomerId",
                table: "supportTickets");

            migrationBuilder.DropIndex(
                name: "IX_supportTickets_CustomerId",
                table: "supportTickets");

            migrationBuilder.DropIndex(
                name: "IX_supportTickets_ServiceId",
                table: "supportTickets");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "supportTickets");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "supportTickets");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "supportTickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_supportTickets_AssignedToId",
                table: "supportTickets",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_supportTickets_AspNetUsers_AssignedToId",
                table: "supportTickets",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
