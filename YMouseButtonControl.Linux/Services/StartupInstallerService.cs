using YMouseButtonControl.Core.Services.StartupInstaller;

namespace YMouseButtonControl.Linux.Services;

public class StartupInstallerService : IStartupInstallerService
{
    private const string DesktopFile = """
        [Desktop Entry]
        Type=Application
        Exec={0}
        Path={1}
        Hidden=false
        NoDisplay=false
        X-GNOME-Autostart-enabled=true
        Name=YMouseButtonControl
        Comment=YMouseButtonControl
        """;
    private readonly string _configDir = Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData
    );
    private readonly string _autostartDir;
    private readonly string _desktopFilePath;

    public StartupInstallerService()
    {
        _autostartDir = Path.Combine(_configDir, "autostart");
        _desktopFilePath = Path.Combine(_autostartDir, "YMouseButtonControl.desktop");
    }

    public bool ButtonEnabled() => Directory.Exists(_autostartDir);

    public bool InstallStatus()
    {
        if (!Directory.Exists(_autostartDir))
        {
            return false;
        }

        if (!File.Exists(_desktopFilePath))
        {
            return false;
        }

        var expectedExecLine = $"Exec={GetCurExePath()}";
        return File.ReadAllText(_desktopFilePath).Contains(expectedExecLine);
    }

    public void Install() =>
        File.WriteAllText(
            _desktopFilePath,
            string.Format(DesktopFile, GetCurExePath(), GetCurExeParentPath())
        );

    public void Uninstall() => File.Delete(_desktopFilePath);

    private static string GetCurExeParentPath() =>
        Path.GetDirectoryName(GetCurExePath())
        ?? throw new Exception("Error retrieving parent of process path");

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
