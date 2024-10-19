using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YMouseButtonControl.Core.Services.Logging;

public interface IEnableLoggingService
{
    void EnableLogging();
    void DisableLogging();

    /// <summary>
    /// True = enabled
    /// False = disabled
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    bool GetLoggingState();
}

public class EnableLoggingService : IEnableLoggingService
{
    private readonly string _pathToAppSettings = Path.Join(
        AppContext.BaseDirectory,
        "appsettings.json"
    );

    /// <summary>
    /// True = enabled
    /// False = disabled
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public bool GetLoggingState()
    {
        if (!File.Exists(_pathToAppSettings))
        {
            throw new Exception($"Appsettings file not found: {_pathToAppSettings}");
        }
        var json =
            JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_pathToAppSettings))
            ?? throw new Exception($"Error deserializing {_pathToAppSettings}");
        return json.GetValue("Logging") != null;
    }

    public void EnableLogging()
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

    public void DisableLogging()
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
