using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3434), "Information Technology Department", "IT" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3438), "Marketing Department", "Marketing" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3442), "Finance Department", "Finance" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3445), "Human Resources Department", "HR" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3449), "Sales Department", "Sales" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3452), "Operations Department", "Operations" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3477), "Operations Department Boss", "Boss-Operations" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3480), "Regular Employee", "Employee" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3457), "Company Boss - Full Access", "Boss" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3461), "IT Department Boss", "Boss-IT" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3464), "Marketing Department Boss", "Boss-Marketing" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3467), "Finance Department Boss", "Boss-Finance" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3471), "HR Department Boss", "Boss-HR" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3474), "Sales Department Boss", "Boss-Sales" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employees");
        }
    }
}
