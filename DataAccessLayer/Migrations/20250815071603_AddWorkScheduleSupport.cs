using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkScheduleSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkScheduleId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RequiredWorkHours = table.Column<int>(type: "int", nullable: false),
                    MinimumWorkMinutes = table.Column<int>(type: "int", nullable: false),
                    MaxLatenessMinutes = table.Column<int>(type: "int", nullable: false),
                    MaxEarlyLeaveMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6703));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6706));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6712));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6715));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6744));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6724));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6728));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6731));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6735));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6738));

            migrationBuilder.InsertData(
                table: "WorkSchedules",
                columns: new[] { "Id", "CreatedAt", "Description", "EndTime", "IsActive", "MaxEarlyLeaveMinutes", "MaxLatenessMinutes", "MinimumWorkMinutes", "Name", "RequiredWorkHours", "StartTime" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999991"), new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6677), "Standard 8:00-17:00 work schedule", new TimeSpan(0, 17, 0, 0, 0), true, 15, 15, 480, "8-17", 8, new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("99999999-9999-9999-9999-999999999992"), new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6683), "Standard 9:00-18:00 work schedule", new TimeSpan(0, 18, 0, 0, 0), true, 15, 15, 480, "9-18", 8, new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("99999999-9999-9999-9999-999999999993"), new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6689), "Morning shift 9:00-14:00", new TimeSpan(0, 14, 0, 0, 0), true, 10, 10, 300, "9-14", 5, new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("99999999-9999-9999-9999-999999999994"), new DateTime(2025, 8, 15, 11, 16, 2, 583, DateTimeKind.Local).AddTicks(6693), "Afternoon shift 14:00-18:00", new TimeSpan(0, 18, 0, 0, 0), true, 10, 10, 240, "14-18", 4, new TimeSpan(0, 14, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_Name",
                table: "WorkSchedules",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkSchedules_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkSchedules_WorkScheduleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WorkScheduleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkScheduleId",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3438));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3442));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3445));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3449));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3452));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3480));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3457));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3461));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3464));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3467));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3471));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 3, 41, 424, DateTimeKind.Local).AddTicks(3474));
        }
    }
}
