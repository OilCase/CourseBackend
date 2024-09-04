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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderInChapter",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "OrderInCourse",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "OrderInPart",
                table: "Chapters");
        }
    }
}