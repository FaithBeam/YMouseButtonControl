using System;
using System.IO;
using WindowsShortcutFactory;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;

public class StartMenuInstallerStatusWindows : IStartMenuInstallerStatus
{
    private readonly string _roamingAppDataFolder = Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData
    );

    private readonly string _roamingYMouseButtonsFolder;

    private readonly string _roamingYmouseButtonsShortcutPath;

    public StartMenuInstallerStatusWindows()
    {
        _roamingYMouseButtonsFolder = Path.Combine(
            _roamingAppDataFolder,
            "Microsoft",
            "Windows",
            "Start Menu",
            "Programs",
            "YMouseButtonControl"
        );
        _roamingYmouseButtonsShortcutPath = Path.Combine(
            _roamingYMouseButtonsFolder,
            "YMouseButtonControl.lnk"
        );
    }

    public bool InstallStatus()
    {
        if (!File.Exists(_roamingYmouseButtonsShortcutPath))
        {
            return false;
        }
        using var shortcut = WindowsShortcut.Load(_roamingYmouseButtonsShortcutPath);
        return shortcut.Path == GetCurExePath();
    }

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
