using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMouseButtonControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectDefaultProfileDisplayPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayPriority",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayPriority",
                value: 1);
        }
    }
}
