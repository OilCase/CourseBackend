using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddQuestionIsContentFilled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10747b75-1dc5-4e9a-8f80-6c641f979957");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efde4772-900d-4b60-bbb6-e1607bf09747");

            migrationBuilder.AddColumn<bool>(
                name: "IsContentFilled",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fde8a0e-51b0-4a81-bc51-abf3a55022e5", null, "Author", "AUTHOR" },
                    { "dad2fe7f-2a80-450e-b0c2-c2e4cd611d76", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fde8a0e-51b0-4a81-bc51-abf3a55022e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dad2fe7f-2a80-450e-b0c2-c2e4cd611d76");

            migrationBuilder.DropColumn(
                name: "IsContentFilled",
                table: "Questions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10747b75-1dc5-4e9a-8f80-6c641f979957", null, "Author", "AUTHOR" },
                    { "efde4772-900d-4b60-bbb6-e1607bf09747", null, "Admin", "ADMIN" }
                });
        }
    }
}
