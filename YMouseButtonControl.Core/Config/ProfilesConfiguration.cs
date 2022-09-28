using System.IO;
using Avalonia.Collections;
using Newtonsoft.Json;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Config;

public static class ProfilesConfiguration
{
    public static AvaloniaList<Profile> Profiles { get; set; }

    public static AvaloniaList<Profile> LoadProfiles()
    {
        Profiles = JsonConvert.DeserializeObject<AvaloniaList<Profile>>(File.ReadAllText("profiles.json"));
        return Profiles;
    }
}