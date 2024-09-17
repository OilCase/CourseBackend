using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Model.AdminPanelMigrations
{
    /// <inheritdoc />
    public partial class AddChapterToTestings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Testings_TestingId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_TestingId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "TestingId",
                table: "Chapters");

            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "Testings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testings_ChapterId",
                table: "Testings",
                column: "ChapterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Testings_Chapters_ChapterId",
                table: "Testings",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testings_Chapters_ChapterId",
                table: "Testings");

            migrationBuilder.DropIndex(
                name: "IX_Testings_ChapterId",
                table: "Testings");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Testings");

            migrationBuilder.AddColumn<int>(
                name: "TestingId",
                table: "Chapters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_TestingId",
                table: "Chapters",
                column: "TestingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Testings_TestingId",
                table: "Chapters",
                column: "TestingId",
                principalTable: "Testings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
