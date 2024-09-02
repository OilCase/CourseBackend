using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class RemoveOrderInFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0273822a-abc3-4430-a0c3-037d301aa87f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "829eaa3e-5f10-4237-a5bb-5b366c4d1eac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cd0afb6-3620-4b73-8765-223e7dc53019");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dfc40882-1e0a-4f2e-97f4-87fbf390051c");

            migrationBuilder.DropColumn(
                name: "OrderInChapter",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "OrderInCourse",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "OrderInPart",
                table: "Chapters");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3042bfff-23d4-48e9-bcb5-a9c6fd45a1d9", null, "value__", "VALUE__" },
                    { "38ae04f6-9f61-4435-9adb-7902efe87977", null, "Regular", "REGULAR" },
                    { "4ec84855-b232-4ebe-81ba-d4bd34990b1c", null, "Author", "AUTHOR" },
                    { "551b514e-2bf8-43bc-b62f-80bbfceaf8cd", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 12, 7, 5, 589, DateTimeKind.Utc).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 12, 7, 5, 589, DateTimeKind.Utc).AddTicks(8115));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 12, 7, 5, 589, DateTimeKind.Utc).AddTicks(8116));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3042bfff-23d4-48e9-bcb5-a9c6fd45a1d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38ae04f6-9f61-4435-9adb-7902efe87977");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ec84855-b232-4ebe-81ba-d4bd34990b1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "551b514e-2bf8-43bc-b62f-80bbfceaf8cd");

            migrationBuilder.AddColumn<int>(
                name: "OrderInChapter",
                table: "Sections",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderInCourse",
                table: "Parts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderInPart",
                table: "Chapters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0273822a-abc3-4430-a0c3-037d301aa87f", null, "Author", "AUTHOR" },
                    { "829eaa3e-5f10-4237-a5bb-5b366c4d1eac", null, "Regular", "REGULAR" },
                    { "9cd0afb6-3620-4b73-8765-223e7dc53019", null, "Admin", "ADMIN" },
                    { "dfc40882-1e0a-4f2e-97f4-87fbf390051c", null, "value__", "VALUE__" }
                });

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 53, 21, 507, DateTimeKind.Utc).AddTicks(6340));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 53, 21, 507, DateTimeKind.Utc).AddTicks(6346));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 53, 21, 507, DateTimeKind.Utc).AddTicks(6346));
        }
    }
}
