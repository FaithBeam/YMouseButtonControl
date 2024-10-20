using YMouseButtonControl.Core.Services.StartMenuInstaller;

namespace YMouseButtonControl.Linux.Services;

public class StartMenuInstallerService : IStartMenuInstallerService
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

    private readonly string _localShare = Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData
    );

    private readonly string _desktopFilePath;

    public StartMenuInstallerService()
    {
        var applicationsDir = Path.Combine(_localShare, "applications");
        _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
    }

    public bool InstallStatus() => File.Exists(_desktopFilePath) &&
                                   File.ReadAllText(_desktopFilePath).Contains($"Exec={GetCurExePath()}");

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