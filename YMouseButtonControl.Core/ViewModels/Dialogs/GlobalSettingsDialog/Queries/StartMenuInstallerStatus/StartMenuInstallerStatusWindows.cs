using System;
using System.IO;
using WindowsShortcutFactory;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;

public static class StartMenuInstallerStatusWindows
{
    public sealed class Handler : IStartMenuInstallerStatusHandler
    {
        public bool Execute()
        {
            var roamingAppDataFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData
            );
            var roamingYMouseButtonsFolder = Path.Combine(
                roamingAppDataFolder,
                "Microsoft",
                "Windows",
                "Start Menu",
                "Programs",
                "YMouseButtonControl"
            );
            var roamingYmouseButtonsShortcutPath = Path.Combine(
                roamingYMouseButtonsFolder,
                "YMouseButtonControl.lnk"
            );
            if (!File.Exists(roamingYmouseButtonsShortcutPath))
            {
                return false;
            }
            using var shortcut = WindowsShortcut.Load(roamingYmouseButtonsShortcutPath);
            return shortcut.Path == GetCurExePath();
        }
    }

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
