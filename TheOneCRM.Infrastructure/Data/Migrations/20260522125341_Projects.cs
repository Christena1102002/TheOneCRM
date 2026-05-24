using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedToId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_customers_customerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_Projects_ProjectId",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Project",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Project",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "AssignedToId",
                table: "Project",
                newName: "ProjectManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_customerId",
                table: "Project",
                newName: "IX_Project_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CreatedById",
                table: "Project",
                newName: "IX_Project_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_AssignedToId",
                table: "Project",
                newName: "IX_Project_ProjectManagerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Project",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProjectEngineers",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    EngineerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEngineers", x => new { x.ProjectId, x.EngineerId });
                    table.ForeignKey(
                        name: "FK_ProjectEngineers_AspNetUsers_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectEngineers_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEngineers_EngineerId",
                table: "ProjectEngineers",
                column: "EngineerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_CreatedById",
                table: "Project",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_customers_CustomerId",
                table: "Project",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_CreatedById",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_ProjectManagerId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_customers_CustomerId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks");

            migrationBuilder.DropTable(
                name: "ProjectEngineers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Projects",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Projects",
                newName: "customerId");

            migrationBuilder.RenameColumn(
                name: "ProjectManagerId",
                table: "Projects",
                newName: "AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Projects",
                newName: "IX_Projects_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_CustomerId",
                table: "Projects",
                newName: "IX_Projects_customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_CreatedById",
                table: "Projects",
                newName: "IX_Projects_CreatedById");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedToId",
                table: "Projects",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_customers_customerId",
                table: "Projects",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_Projects_ProjectId",
                table: "tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
