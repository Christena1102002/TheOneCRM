using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheOneCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_AspNetUsers_AssignedToId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_AspNetUsers_CreatedById",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedHours",
                table: "tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_AspNetUsers_AssignedToId",
                table: "tasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_AspNetUsers_CreatedById",
                table: "tasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_AspNetUsers_AssignedToId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_AspNetUsers_CreatedById",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "EstimatedHours",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_AspNetUsers_AssignedToId",
                table: "tasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_AspNetUsers_CreatedById",
                table: "tasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_Project_ProjectId",
                table: "tasks",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
