﻿using System;
using System.IO;
using WindowsShortcutFactory;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.StartMenuInstall;

public static class StartMenuInstallWindows
{
    public sealed class Handler : IStartMenuInstallHandler
    {
        private readonly string _roamingAppDataFolder = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );

        private readonly string _roamingYMouseButtonsFolder;

        private readonly string _roamingYmouseButtonsShortcutPath;

        public Handler()
        {
            _roamingYMouseButtonsFolder = Path.Combine(
                _roamingAppDataFolder,
                "Microsoft",
                "Windows",
                "Start Menu",
                "Programs",
                "YMouseButtonControl"
            );
            _roamingYmouseButtonsShortcutPath = Path.Combine(
                _roamingYMouseButtonsFolder,
                "YMouseButtonControl.lnk"
            );
        }

        public void Execute()
        {
            if (File.Exists(_roamingYmouseButtonsShortcutPath))
            {
                File.Delete(_roamingYmouseButtonsShortcutPath);
            }

            if (!Directory.Exists(_roamingYMouseButtonsFolder))
            {
                Directory.CreateDirectory(_roamingYMouseButtonsFolder);
            }

            using var shortcut = new WindowsShortcut();
            shortcut.Path = GetCurExePath();
            shortcut.WorkingDirectory = GetCurExeParentPath();
            shortcut.Save(_roamingYmouseButtonsShortcutPath);
        }

        private static string GetCurExeParentPath() =>
            Path.GetDirectoryName(GetCurExePath())
            ?? throw new Exception("Error retrieving parent of process path");

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
