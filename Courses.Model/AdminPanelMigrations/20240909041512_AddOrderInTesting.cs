using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddOrderInTesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderInTesting",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShowFullTitle",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderInTesting",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ShowFullTitle",
                table: "Questions");
        }
    }
}
