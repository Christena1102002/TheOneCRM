using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedcountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_priceQuotationDetails_Services_ServiceId",
                table: "priceQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_priceQuotationDetails_priceQuotations_PriceQuotationId",
                table: "priceQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_priceQuotations_customers_CustomerId",
                table: "priceQuotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_priceQuotations",
                table: "priceQuotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_priceQuotationDetails",
                table: "priceQuotationDetails");

            migrationBuilder.RenameTable(
                name: "priceQuotations",
                newName: "PriceQuotations");

            migrationBuilder.RenameTable(
                name: "priceQuotationDetails",
                newName: "PriceQuotationDetails");

            migrationBuilder.RenameIndex(
                name: "IX_priceQuotations_CustomerId",
                table: "PriceQuotations",
                newName: "IX_PriceQuotations_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_priceQuotationDetails_ServiceId",
                table: "PriceQuotationDetails",
                newName: "IX_PriceQuotationDetails_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_priceQuotationDetails_PriceQuotationId",
                table: "PriceQuotationDetails",
                newName: "IX_PriceQuotationDetails_PriceQuotationId");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CampaignCountry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceQuotations",
                table: "PriceQuotations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceQuotationDetails",
                table: "PriceQuotationDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotationDetails_PriceQuotations_PriceQuotationId",
                table: "PriceQuotationDetails",
                column: "PriceQuotationId",
                principalTable: "PriceQuotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotationDetails_Services_ServiceId",
                table: "PriceQuotationDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_customers_CustomerId",
                table: "PriceQuotations",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotationDetails_PriceQuotations_PriceQuotationId",
                table: "PriceQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotationDetails_Services_ServiceId",
                table: "PriceQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_customers_CustomerId",
                table: "PriceQuotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceQuotations",
                table: "PriceQuotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceQuotationDetails",
                table: "PriceQuotationDetails");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CampaignCountry");

            migrationBuilder.RenameTable(
                name: "PriceQuotations",
                newName: "priceQuotations");

            migrationBuilder.RenameTable(
                name: "PriceQuotationDetails",
                newName: "priceQuotationDetails");

            migrationBuilder.RenameIndex(
                name: "IX_PriceQuotations_CustomerId",
                table: "priceQuotations",
                newName: "IX_priceQuotations_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceQuotationDetails_ServiceId",
                table: "priceQuotationDetails",
                newName: "IX_priceQuotationDetails_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceQuotationDetails_PriceQuotationId",
                table: "priceQuotationDetails",
                newName: "IX_priceQuotationDetails_PriceQuotationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_priceQuotations",
                table: "priceQuotations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_priceQuotationDetails",
                table: "priceQuotationDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_priceQuotationDetails_Services_ServiceId",
                table: "priceQuotationDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_priceQuotationDetails_priceQuotations_PriceQuotationId",
                table: "priceQuotationDetails",
                column: "PriceQuotationId",
                principalTable: "priceQuotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_priceQuotations_customers_CustomerId",
                table: "priceQuotations",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
