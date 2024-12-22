using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.IsInstalled;

public static class IsInstalledLinux
{
    public sealed class Handler : IIsInstalledHandler
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

        public bool Execute()
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

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
