using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.Logging;

public static class EnableLogging
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
            var toAdd = new JProperty(
                "Logging",
                new JObject(
                    new JProperty(
                        "LogLevel",
                        new JObject(
                            new JProperty("Default", "Debug"),
                            new JProperty("System", "Information"),
                            new JProperty("Microsoft", "Error")
                        )
                    ),
                    new JProperty(
                        "File",
                        new JObject(
                            new JProperty("Path", "YMouseButtonControl.log"),
                            new JProperty("Append", true),
                            new JProperty("MinLevel", "Information"),
                            new JProperty("FileSizeLimitBytes", 0),
                            new JProperty("MaxRollingFiles", 0)
                        )
                    )
                )
            );
            json.Add(toAdd);
            File.WriteAllText(
                _pathToAppSettings,
                JsonConvert.SerializeObject(json, Formatting.Indented)
            );
        }
    }
}
