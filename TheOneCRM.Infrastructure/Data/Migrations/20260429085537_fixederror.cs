using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixederror : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_campaigns_AspNetUsers_AppUserId",
                table: "campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_customers_campaigns_campaignsId",
                table: "customers");

            migrationBuilder.DropIndex(
                name: "IX_customers_campaignsId",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "statusOfDeal",
                table: "deals");

            migrationBuilder.DropColumn(
                name: "IsActiveCustomer",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "campaignsId",
                table: "customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerStatus",
                table: "deals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CampanyName",
                table: "customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customerServices",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerServices", x => new { x.customerId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_customerServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_customerServices_customers_customerId",
                        column: x => x.customerId,
                        principalTable: "customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_compaignId",
                table: "customers",
                column: "compaignId");

            migrationBuilder.CreateIndex(
                name: "IX_customerServices_ServiceId",
                table: "customerServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_campaigns_AspNetUsers_AppUserId",
                table: "campaigns",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_customers_campaigns_compaignId",
                table: "customers",
                column: "compaignId",
                principalTable: "campaigns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_campaigns_AspNetUsers_AppUserId",
                table: "campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_customers_campaigns_compaignId",
                table: "customers");

            migrationBuilder.DropTable(
                name: "customerServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_customers_compaignId",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "CustomerStatus",
                table: "deals");

            migrationBuilder.DropColumn(
                name: "CampanyName",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "customers");

            migrationBuilder.AddColumn<int>(
                name: "statusOfDeal",
                table: "deals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveCustomer",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "campaignsId",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_customers_campaignsId",
                table: "customers",
                column: "campaignsId");

            migrationBuilder.AddForeignKey(
                name: "FK_campaigns_AspNetUsers_AppUserId",
                table: "campaigns",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customers_campaigns_campaignsId",
                table: "customers",
                column: "campaignsId",
                principalTable: "campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
