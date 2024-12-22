using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;

public class StartMenuInstallerStatusLinux : IStartMenuInstallerStatus
{
    private readonly string _localShare = Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData
    );
    private readonly string _desktopFilePath;

    public StartMenuInstallerStatusLinux()
    {
        var applicationsDir = Path.Combine(_localShare, "applications");
        _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
    }

    public bool InstallStatus() =>
        File.Exists(_desktopFilePath)
        && File.ReadAllText(_desktopFilePath).Contains($"Exec={GetCurExePath()}");

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
