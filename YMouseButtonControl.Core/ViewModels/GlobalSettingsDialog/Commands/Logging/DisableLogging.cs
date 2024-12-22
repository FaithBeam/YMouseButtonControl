using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.Logging;

public static class DisableLogging
{
    public sealed class Handler
    {
        private readonly string _pathToAppSettings = Path.Join(
            AppContext.BaseDirectory,
            "appsettings.json"
        );

        public void Execute()
        {
            if (!File.Exists(_pathToAppSettings))
            {
                throw new Exception($"Appsettings file not found: {_pathToAppSettings}");
            }

            var json =
                JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_pathToAppSettings))
                ?? throw new Exception($"Error deserializing {_pathToAppSettings}");
            json.Remove("Logging");
            File.WriteAllText(
                _pathToAppSettings,
                JsonConvert.SerializeObject(json, Formatting.Indented)
            );
        }
    }
}
