using System;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace YMouseButtonControl.Core.ViewModels.App.Commands.StartupInstaller.Uninstall;

[SupportedOSPlatform("windows")]
public static class UninstallWindows
{
    public sealed class Handler : IUninstallHandler
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
            key.DeleteValue(ValName);
        }
    }
}
