using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class RemoveHeadingDescriptionIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Headings_Description",
                table: "Headings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Headings_Description",
                table: "Headings",
                column: "Description",
                unique: true);
        }
    }
}
