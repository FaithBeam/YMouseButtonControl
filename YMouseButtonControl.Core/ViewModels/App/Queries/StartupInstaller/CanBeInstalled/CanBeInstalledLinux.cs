using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.App.Queries.StartupInstaller.CanBeInstalled;

public static class CanBeInstalledLinux
{
    public sealed class Handler : ICanBeInstalledHandler
    {
        private readonly string _configDir = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );
        private readonly string _autostartDir;

        public Handler()
        {
            _autostartDir = Path.Combine(_configDir, "autostart");
        }

        public bool Execute() => Directory.Exists(_autostartDir);
    }
}
