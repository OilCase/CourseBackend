using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddLabelNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Labels_Name",
                table: "Labels",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_Name",
                table: "Labels");
        }
    }
}