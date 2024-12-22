using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuUninstall;

public class StartMenuUninstallWindows : IStartMenuUninstall
{
    private readonly string _roamingAppDataFolder = Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData
    );
    private readonly string _roamingYMouseButtonsFolder;
    private readonly string _roamingYmouseButtonsShortcutPath;

    public StartMenuUninstallWindows()
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

    public void Uninstall() => File.Delete(_roamingYmouseButtonsShortcutPath);
}
