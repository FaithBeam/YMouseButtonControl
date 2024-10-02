using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMouseButtonControl.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedToButtonMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "ButtonMappings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 2,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 3,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 4,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 5,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 6,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 7,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 8,
                column: "Selected",
                value: false);

            migrationBuilder.UpdateData(
                table: "ButtonMappings",
                keyColumn: "Id",
                keyValue: 9,
                column: "Selected",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "ButtonMappings");
        }
    }
}
