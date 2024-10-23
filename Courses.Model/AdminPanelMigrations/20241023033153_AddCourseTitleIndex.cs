using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddCourseTitleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_Title",
                table: "Courses",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_Title",
                table: "Courses");
        }
    }
}
