using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ResolvedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "supportTickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "supportTickets");
        }
    }
}
