using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migrationnnNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxEarlyLeaveMinutes",
                table: "WorkSchedules");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2407));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2409));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2411));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2414));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2416));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999991"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2377));

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999992"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2381));

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999993"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999994"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 14, 19, 48, 214, DateTimeKind.Local).AddTicks(2387));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxEarlyLeaveMinutes",
                table: "WorkSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5925));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5929));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5932));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5935));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5939));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5942));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5967));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5970));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5948));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5951));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5954));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5958));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5961));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5964));

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999991"),
                columns: new[] { "CreatedAt", "MaxEarlyLeaveMinutes" },
                values: new object[] { new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5903), 15 });

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999992"),
                columns: new[] { "CreatedAt", "MaxEarlyLeaveMinutes" },
                values: new object[] { new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5910), 15 });

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999993"),
                columns: new[] { "CreatedAt", "MaxEarlyLeaveMinutes" },
                values: new object[] { new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5914), 10 });

            migrationBuilder.UpdateData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999994"),
                columns: new[] { "CreatedAt", "MaxEarlyLeaveMinutes" },
                values: new object[] { new DateTime(2025, 8, 18, 10, 32, 45, 705, DateTimeKind.Local).AddTicks(5919), 10 });
        }
    }
}
