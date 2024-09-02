using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class OrderInFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "007a05d8-e2b6-4838-8415-4a8e1164a569");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6841bfac-9480-45ae-8a56-2fe7cf4eb639");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc11c2a2-b755-45b7-9764-1dfa8b942c26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3c1e619-7b2b-460d-ab1a-598bd995b5a8");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "007a05d8-e2b6-4838-8415-4a8e1164a569", null, "Author", "AUTHOR" },
                    { "6841bfac-9480-45ae-8a56-2fe7cf4eb639", null, "Admin", "ADMIN" },
                    { "bc11c2a2-b755-45b7-9764-1dfa8b942c26", null, "value__", "VALUE__" },
                    { "e3c1e619-7b2b-460d-ab1a-598bd995b5a8", null, "Regular", "REGULAR" }
                });

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 47, 48, 624, DateTimeKind.Utc).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 47, 48, 624, DateTimeKind.Utc).AddTicks(3419));

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastChangeDateTime",
                value: new DateTime(2024, 9, 2, 9, 47, 48, 624, DateTimeKind.Utc).AddTicks(3420));
        }
    }
}
