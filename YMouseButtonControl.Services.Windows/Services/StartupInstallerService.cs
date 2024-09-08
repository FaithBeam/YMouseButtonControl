using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using Microsoft.Win32;
using YMouseButtonControl.Core.Services;

namespace YMouseButtonControl.Services.Windows.Services;

[SupportedOSPlatform("windows")]
public class StartupInstallerService : IStartupInstallerService
{
    private const string BaseKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\";
    private const string ValName = "YMouseButtonControl";

    public bool ButtonEnabled() => true;

    /// <summary>
    /// Gets the start-up install status of the program
    /// </summary>
    /// <returns>True = installed, False = not installed</returns>
    public bool InstallStatus()
    {
        using var key = Registry.CurrentUser.OpenSubKey(BaseKeyPath, RegistryRights.ReadKey);
        var keyVal = key?.GetValue(ValName)?.ToString();
        if (string.IsNullOrWhiteSpace(keyVal))
        {
            return false;
        }

        return keyVal.Trim('"') == GetCurExePath();
    }

    public void Install()
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

    public void Uninstall()
    {
        using var key =
            Registry.CurrentUser.OpenSubKey(
                BaseKeyPath,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl
            ) ?? throw new Exception($"Error opening key {BaseKeyPath}");
        key.DeleteValue(ValName);
    }

    private static string GetCurExePath() =>
        Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
}
