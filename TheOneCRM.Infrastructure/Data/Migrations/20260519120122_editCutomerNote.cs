using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editCutomerNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_CreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerNotes_CreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CustomerNotes");

            migrationBuilder.AddColumn<DateTime>(
                name: "MarketingCreatedAt",
                table: "CustomerNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingCreatedById",
                table: "CustomerNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SalesCreatedAt",
                table: "CustomerNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesCreatedById",
                table: "CustomerNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupportCreatedAt",
                table: "CustomerNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportCreatedById",
                table: "CustomerNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_MarketingCreatedById",
                table: "CustomerNotes",
                column: "MarketingCreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_SalesCreatedById",
                table: "CustomerNotes",
                column: "SalesCreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_SupportCreatedById",
                table: "CustomerNotes",
                column: "SupportCreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_MarketingCreatedById",
                table: "CustomerNotes",
                column: "MarketingCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_SalesCreatedById",
                table: "CustomerNotes",
                column: "SalesCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_SupportCreatedById",
                table: "CustomerNotes",
                column: "SupportCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_MarketingCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_SalesCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_SupportCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerNotes_MarketingCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerNotes_SalesCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerNotes_SupportCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "MarketingCreatedAt",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "MarketingCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "SalesCreatedAt",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "SalesCreatedById",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "SupportCreatedAt",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "SupportCreatedById",
                table: "CustomerNotes");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CustomerNotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_CreatedById",
                table: "CustomerNotes",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_AspNetUsers_CreatedById",
                table: "CustomerNotes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
