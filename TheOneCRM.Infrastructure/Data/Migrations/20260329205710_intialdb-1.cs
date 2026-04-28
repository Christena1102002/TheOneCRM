using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intialdb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deals_leads_customerId",
                table: "deals");

            migrationBuilder.DropForeignKey(
                name: "FK_leads_AspNetUsers_AssignedToId",
                table: "leads");

            migrationBuilder.DropForeignKey(
                name: "FK_leads_AspNetUsers_CreatedById",
                table: "leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_leads_customerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_leads",
                table: "leads");

            migrationBuilder.RenameTable(
                name: "leads",
                newName: "customers");

            migrationBuilder.RenameIndex(
                name: "IX_leads_CreatedById",
                table: "customers",
                newName: "IX_customers_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_leads_AssignedToId",
                table: "customers",
                newName: "IX_customers_AssignedToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_customers_AspNetUsers_AssignedToId",
                table: "customers",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_customers_AspNetUsers_CreatedById",
                table: "customers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_deals_customers_customerId",
                table: "deals",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_customers_customerId",
                table: "Projects",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customers_AspNetUsers_AssignedToId",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "FK_customers_AspNetUsers_CreatedById",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "FK_deals_customers_customerId",
                table: "deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_customers_customerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "leads");

            migrationBuilder.RenameIndex(
                name: "IX_customers_CreatedById",
                table: "leads",
                newName: "IX_leads_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_customers_AssignedToId",
                table: "leads",
                newName: "IX_leads_AssignedToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_leads",
                table: "leads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_deals_leads_customerId",
                table: "deals",
                column: "customerId",
                principalTable: "leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_AspNetUsers_AssignedToId",
                table: "leads",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_leads_AspNetUsers_CreatedById",
                table: "leads",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_leads_customerId",
                table: "Projects",
                column: "customerId",
                principalTable: "leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
