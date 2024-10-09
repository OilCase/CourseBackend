using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddDirectionDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "test");

            migrationBuilder.AddColumn<int>(
                name: "DescriptionId",
                table: "Directions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DescriptionId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DescriptionId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Directions",
                keyColumn: "Id",
                keyValue: 3,
                column: "DescriptionId",
                value: 6);

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[] { "fr", "Français" });

            migrationBuilder.InsertData(
                table: "Localizations",
                column: "Id",
                values: new object[]
                {
                    4,
                    5,
                    6
                });

            migrationBuilder.InsertData(
                table: "LocalizationValues",
                columns: new[] { "Id", "LanguageId", "LocalizationId", "Value" },
                values: new object[,]
                {
                    { 7, "en", 4, "Shelf Description" },
                    { 8, "ru", 4, "шельфовое описание" },
                    { 9, "en", 5, "Geology Description" },
                    { 10, "ru", 5, "геологичное описание" },
                    { 11, "en", 6, "Drilling Description" },
                    { 12, "ru", 6, "бурительное описание" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directions_DescriptionId",
                table: "Directions",
                column: "DescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_Localizations_DescriptionId",
                table: "Directions",
                column: "DescriptionId",
                principalTable: "Localizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directions_Localizations_DescriptionId",
                table: "Directions");

            migrationBuilder.DropIndex(
                name: "IX_Directions_DescriptionId",
                table: "Directions");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "fr");

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LocalizationValues",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Localizations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Localizations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Localizations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "Directions");

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[] { "test", "Тестовский" });
        }
    }
}
