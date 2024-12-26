using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YMouseButtonControl.Infrastructure.Migrations
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
                    BoolValue = table.Column<bool>(type: "INTEGER", nullable: true),
                    IntValue = table.Column<int>(type: "INTEGER", nullable: true),
                    StringValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Background = table.Column<string>(type: "TEXT", nullable: false),
                    Highlight = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
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
                    AutoRepeatDelay = table.Column<int>(type: "INTEGER", nullable: true),
                    AutoRepeatRandomizeDelayEnabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    Selected = table.Column<bool>(type: "INTEGER", nullable: false),
                    BlockOriginalMouseInput = table.Column<bool>(type: "INTEGER", nullable: true),
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
                columns: new[] { "Id", "BoolValue", "Discriminator", "Name" },
                values: new object[] { 1, false, "SettingBool", "StartMinimized" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Discriminator", "IntValue", "Name" },
                values: new object[] { 2, "SettingInt", 3, "Theme" });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "Background", "Highlight", "Name" },
                values: new object[,]
                {
                    { 1, "SystemAltHighColor", "SystemAccentColor", "Default" },
                    { 2, "White", "Yellow", "Light" },
                    { 3, "Black", "#3700b3", "Dark" }
                });

            migrationBuilder.InsertData(
                table: "ButtonMappings",
                columns: new[] { "Id", "AutoRepeatDelay", "AutoRepeatRandomizeDelayEnabled", "BlockOriginalMouseInput", "Discriminator", "Keys", "MouseButton", "ProfileId", "Selected", "SimulatedKeystrokeType" },
                values: new object[,]
                {
                    { 1, null, null, null, "NothingMapping", null, 0, 1, true, null },
                    { 2, null, null, null, "DisabledMapping", null, 0, 1, false, null },
                    { 3, null, null, true, "SimulatedKeystroke", null, 0, 1, false, null },
                    { 4, null, null, null, "RightClick", null, 0, 1, false, null },
                    { 5, null, null, null, "NothingMapping", null, 1, 1, true, null },
                    { 6, null, null, null, "DisabledMapping", null, 1, 1, false, null },
                    { 7, null, null, true, "SimulatedKeystroke", null, 1, 1, false, null },
                    { 8, null, null, null, "RightClick", null, 1, 1, false, null },
                    { 9, null, null, null, "NothingMapping", null, 2, 1, true, null },
                    { 10, null, null, null, "DisabledMapping", null, 2, 1, false, null },
                    { 11, null, null, true, "SimulatedKeystroke", null, 2, 1, false, null },
                    { 12, null, null, null, "RightClick", null, 2, 1, false, null },
                    { 13, null, null, null, "NothingMapping", null, 3, 1, true, null },
                    { 14, null, null, null, "DisabledMapping", null, 3, 1, false, null },
                    { 15, null, null, true, "SimulatedKeystroke", null, 3, 1, false, null },
                    { 16, null, null, null, "RightClick", null, 3, 1, false, null },
                    { 17, null, null, null, "NothingMapping", null, 4, 1, true, null },
                    { 18, null, null, null, "DisabledMapping", null, 4, 1, false, null },
                    { 19, null, null, true, "SimulatedKeystroke", null, 4, 1, false, null },
                    { 20, null, null, null, "RightClick", null, 4, 1, false, null },
                    { 21, null, null, null, "NothingMapping", null, 5, 1, true, null },
                    { 22, null, null, null, "DisabledMapping", null, 5, 1, false, null },
                    { 23, null, null, true, "SimulatedKeystroke", null, 5, 1, false, null },
                    { 24, null, null, null, "RightClick", null, 5, 1, false, null },
                    { 25, null, null, null, "NothingMapping", null, 6, 1, true, null },
                    { 26, null, null, null, "DisabledMapping", null, 6, 1, false, null },
                    { 27, null, null, true, "SimulatedKeystroke", null, 6, 1, false, null },
                    { 28, null, null, null, "RightClick", null, 6, 1, false, null },
                    { 29, null, null, null, "NothingMapping", null, 7, 1, true, null },
                    { 30, null, null, null, "DisabledMapping", null, 7, 1, false, null },
                    { 31, null, null, true, "SimulatedKeystroke", null, 7, 1, false, null },
                    { 32, null, null, null, "RightClick", null, 7, 1, false, null },
                    { 33, null, null, null, "NothingMapping", null, 8, 1, true, null },
                    { 34, null, null, null, "DisabledMapping", null, 8, 1, false, null },
                    { 35, null, null, true, "SimulatedKeystroke", null, 8, 1, false, null },
                    { 36, null, null, null, "RightClick", null, 8, 1, false, null }
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
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
