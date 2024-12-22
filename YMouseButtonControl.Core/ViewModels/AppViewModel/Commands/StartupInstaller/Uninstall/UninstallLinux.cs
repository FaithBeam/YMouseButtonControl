using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Commands.StartupInstaller.Uninstall;

public static class UninstallLinux
{
    public sealed class Handler : IUninstallHandler
    {
        private readonly string _configDir = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );
        private readonly string _autostartDir;
        private readonly string _desktopFilePath;

        public Handler()
        {
            _autostartDir = Path.Combine(_configDir, "autostart");
            _desktopFilePath = Path.Combine(_autostartDir, "YMouseButtonControl.desktop");
        }

        public void Execute() => File.Delete(_desktopFilePath);
    }
}
