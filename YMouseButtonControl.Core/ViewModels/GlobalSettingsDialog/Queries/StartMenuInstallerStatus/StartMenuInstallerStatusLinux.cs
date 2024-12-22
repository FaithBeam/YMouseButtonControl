using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;

public static class StartMenuInstallerStatusLinux
{
    public sealed class Handler : IStartMenuInstallerStatusHandler
    {
        private readonly string _localShare = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
        );

        public bool Execute()
        {
            var applicationsDir = Path.Combine(_localShare, "applications");
            var desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
            return File.Exists(desktopFilePath)
                && File.ReadAllText(desktopFilePath).Contains($"Exec={GetCurExePath()}");
        }

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
