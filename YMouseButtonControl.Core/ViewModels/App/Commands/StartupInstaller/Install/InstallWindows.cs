using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace YMouseButtonControl.Core.ViewModels.App.Commands.StartupInstaller.Install;

[SupportedOSPlatform("windows")]
public static class InstallWindows
{
    public sealed class Handler : IInstallHandler
    {
        private const string BaseKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\";
        private const string ValName = "YMouseButtonControl";

        public void Execute()
        {
            using var key =
                Registry.CurrentUser.OpenSubKey(
                    BaseKeyPath,
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl
                ) ?? throw new Exception($"Error opening key {BaseKeyPath}");
            var newKeyVal = $"\"{Path.Join(GetCurExePath())}\"";
            key.SetValue(ValName, newKeyVal);
        }

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
