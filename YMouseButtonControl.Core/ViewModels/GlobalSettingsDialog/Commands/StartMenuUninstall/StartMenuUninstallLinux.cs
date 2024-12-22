using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuUninstall;

public static class StartMenuUninstallLinux
{
    public sealed class Handler : IStartMenuUninstallHandler
    {
        private readonly string _localShare = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
        );

        private readonly string _desktopFilePath;

        public Handler()
        {
            var applicationsDir = Path.Combine(_localShare, "applications");
            _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
        }

        public void Execute() => File.Delete(_desktopFilePath);
    }
}
