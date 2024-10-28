using Dapper;

namespace YMouseButtonControl.DataAccess.Context;

public class YMouseButtonControlDbContext(IConnectionProvider connectionProvider)
{
    public void Init()
    {
        using var conn = connectionProvider.CreateConnection();
        InitProfiles();
        InitSettings();
        InitThemes();
        InitButtonMappings();
        return;

        void InitProfiles()
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS
                Profiles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                IsDefault BOOLEAN,
                Checked BOOLEAN,
                DisplayPriority INTEGER,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                WindowCaption TEXT NOT NULL,
                Process TEXT NOT NULL,
                WindowClass TEXT NOT NULL,
                ParentClass TEXT NOT NULL,
                MatchType TEXT NOT NULL
                );
                INSERT OR IGNORE INTO Profiles (
                Id, IsDefault, Checked, Name, Description, WindowCaption, Process, WindowClass, ParentClass, MatchType)
                VALUES ('1', '1', '1', 'Default', 'Default description', 'N/A', '*', 'N/A', 'N/A', 'N/A');
                """;
            conn.Execute(sql);
        }

        void InitSettings()
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS
                Settings (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    BoolValue BOOLEAN,
                    StringValue TEXT,
                    IntValue INTEGER,
                    SettingType INTEGER NOT NULL
                );
                INSERT OR IGNORE INTO Settings (Id, Name, BoolValue, SettingType) VALUES ('1', 'StartMinimized', 0, '1');
                INSERT OR IGNORE INTO Settings (Id, Name, IntValue, SettingType) VALUES ('2', 'Theme', '1', '3');
                """;
            conn.Execute(sql);
        }

        void InitThemes()
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS
                Themes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Background TEXT NOT NULL,
                    Highlight TEXT NOT NULL
                );
                INSERT OR IGNORE INTO Themes (Id, Name, Background, Highlight) VALUES ('1', 'Default', 'SystemAltHighColor','SystemAccentColor');
                INSERT OR IGNORE INTO Themes (Id, Name, Background, Highlight) VALUES ('2', 'Light', 'White', 'Yellow');
                INSERT OR IGNORE INTO Themes (Id, Name, Background, Highlight) VALUES ('3', 'Dark', 'Black', '#3700b3');
                """;
            conn.Execute(sql);
        }

        void InitButtonMappings()
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS
                ButtonMappings (
                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Keys TEXT,
                    MouseButton INTEGER NOT NULL,
                    ProfileId INTEGER NOT NULL,
                    SimulatedKeystrokeType INTEGER,
                    Selected BOOLEAN,
                    BlockOriginalMouseInput BOOLEAN,
                    AutoRepeatDelay INTEGER,
                    AutoRepeatRandomizeDelayEnabled BOOLEAN,
                    ButtonMappingType INTEGER NOT NULL,
                    FOREIGN KEY (ProfileId) REFERENCES Profiles (Id) ON DELETE CASCADE
                );
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('1', '0', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('2', '1', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('3', '2', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('4', '3', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('5', '4', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('6', '5', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('7', '6', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('8', '7', '1', '1');
                INSERT OR IGNORE INTO ButtonMappings (Id, MouseButton, ProfileId, ButtonMappingType) VALUES ('9', '8', '1', '1');
                """;
            conn.Execute(sql);
        }
    }
}
