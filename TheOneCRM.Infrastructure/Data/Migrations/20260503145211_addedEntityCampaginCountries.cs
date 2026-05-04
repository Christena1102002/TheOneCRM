using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedEntityCampaginCountries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignCountry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCountry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignCountry_campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCountry_CampaignId",
                table: "CampaignCountry",
                column: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignCountry");
        }
    }
}
