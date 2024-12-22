using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.CanBeInstalled;

public static class CanBeInstalledOsx
{
    public sealed class Handler : ICanBeInstalledHandler
    {
        private readonly string _usrLaunchAgentsDir;

        public Handler()
        {
            _usrLaunchAgentsDir = Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Library",
                "LaunchAgents"
            );
        }

        public bool Execute() => Directory.Exists(_usrLaunchAgentsDir);
    }
}
