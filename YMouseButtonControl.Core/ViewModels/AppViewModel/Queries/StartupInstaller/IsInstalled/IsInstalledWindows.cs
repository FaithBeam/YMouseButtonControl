using System;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.IsInstalled;

public static class IsInstalledWindows
{
    [SupportedOSPlatform("windows")]
    public sealed class Handler : IIsInstalledHandler
    {
        private const string BaseKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\";
        private const string ValName = "YMouseButtonControl";

        public bool Execute()
        {
            using var key = Registry.CurrentUser.OpenSubKey(BaseKeyPath, RegistryRights.ReadKey);
            var keyVal = key?.GetValue(ValName)?.ToString();
            if (string.IsNullOrWhiteSpace(keyVal))
            {
                return false;
            }

            return keyVal.Trim('"') == GetCurExePath();
        }

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
