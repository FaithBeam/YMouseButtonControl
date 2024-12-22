using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuInstall;

public class StartMenuInstallLinux : IStartMenuInstall
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

    public StartMenuInstallLinux()
    {
        var applicationsDir = Path.Combine(_localShare, "applications");
        _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
    }

    public void Install() =>
        File.WriteAllText(
            _desktopFilePath,
            string.Format(DesktopFile, GetCurExePath(), GetCurExeParentPath())
        );

    private static string GetCurExeParentPath() =>
        Path.GetDirectoryName(GetCurExePath())
        ?? throw new Exception("Error retrieving parent of process path");

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
