using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YMouseButtonControl.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    Checked = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayPriority = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    WindowCaption = table.Column<string>(type: "TEXT", nullable: false),
                    Process = table.Column<string>(type: "TEXT", nullable: false),
                    WindowClass = table.Column<string>(type: "TEXT", nullable: false),
                    ParentClass = table.Column<string>(type: "TEXT", nullable: false),
                    MatchType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Value = table.Column<bool>(type: "INTEGER", nullable: true),
                    SettingString_Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ButtonMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Keys = table.Column<string>(type: "TEXT", nullable: true),
                    MouseButton = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    SimulatedKeystrokeType = table.Column<int>(type: "INTEGER", nullable: true),
                    BlockOriginalMouseInput = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ButtonMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ButtonMappings_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "Checked", "Description", "DisplayPriority", "IsDefault", "MatchType", "Name", "ParentClass", "Process", "WindowCaption", "WindowClass" },
                values: new object[] { 1, true, "Default description", 0, true, "N/A", "Default", "N/A", "*", "N/A", "N/A" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Discriminator", "Name", "Value" },
                values: new object[] { 1, "SettingBool", "StartMinimized", false });

            migrationBuilder.InsertData(
                table: "ButtonMappings",
                columns: new[] { "Id", "BlockOriginalMouseInput", "Discriminator", "Keys", "MouseButton", "ProfileId", "SimulatedKeystrokeType" },
                values: new object[,]
                {
                    { 1, false, "NothingMapping", null, 0, 1, null },
                    { 2, false, "NothingMapping", null, 1, 1, null },
                    { 3, false, "NothingMapping", null, 2, 1, null },
                    { 4, true, "SimulatedKeystroke", "ABC123", 3, 1, 7 },
                    { 5, false, "NothingMapping", null, 4, 1, null },
                    { 6, false, "NothingMapping", null, 5, 1, null },
                    { 7, false, "NothingMapping", null, 6, 1, null },
                    { 8, false, "NothingMapping", null, 7, 1, null },
                    { 9, false, "NothingMapping", null, 8, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ButtonMappings_ProfileId",
                table: "ButtonMappings",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ButtonMappings");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
