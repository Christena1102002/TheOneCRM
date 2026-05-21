using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NextFollowUpDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextFollowUpDate",
                table: "supportTickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextFollowUpDate",
                table: "supportTickets");
        }
    }
}
