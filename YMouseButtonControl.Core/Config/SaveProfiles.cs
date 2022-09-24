using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.Config;

public static class SaveProfiles
{
    public static void Save(IList<Profile> profiles)
    {
        File.WriteAllText(@"profiles.json", JsonConvert.SerializeObject(profiles));
    }
}