using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedEntityServuces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Source",
                table: "campaigns",
                newName: "ChannelSourceId");

            migrationBuilder.CreateTable(
                name: "channelSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channelSources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_campaigns_ChannelSourceId",
                table: "campaigns",
                column: "ChannelSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_campaigns_channelSources_ChannelSourceId",
                table: "campaigns",
                column: "ChannelSourceId",
                principalTable: "channelSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_campaigns_channelSources_ChannelSourceId",
                table: "campaigns");

            migrationBuilder.DropTable(
                name: "channelSources");

            migrationBuilder.DropIndex(
                name: "IX_campaigns_ChannelSourceId",
                table: "campaigns");

            migrationBuilder.RenameColumn(
                name: "ChannelSourceId",
                table: "campaigns",
                newName: "Source");
        }
    }
}
