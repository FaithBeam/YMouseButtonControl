using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.StartMenuUninstall;

public static class StartMenuUninstallWindows
{
    public sealed class Handler : IStartMenuUninstallHandler
    {
        private readonly string _roamingAppDataFolder = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );
        private readonly string _roamingYMouseButtonsFolder;
        private readonly string _roamingYmouseButtonsShortcutPath;

        public Handler()
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

        public void Execute()
        {
            File.Delete(_roamingYmouseButtonsShortcutPath);
        }
    }
}
