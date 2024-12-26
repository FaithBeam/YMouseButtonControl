using System.IO;
using DynamicData;
using Newtonsoft.Json;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class ImportProfile
{
    public sealed record Command(string Path);

    public sealed class Handler(IProfilesCache profilesCache, ProfileVmConverter profileVmConverter)
    {
        public void Execute(Command c)
        {
            var f = File.ReadAllText(c.Path);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            settings.Converters.Add(profileVmConverter);
            var deserializedProfile =
                JsonConvert.DeserializeObject<ProfileVm>(f, settings)
                ?? throw new JsonSerializationException("Error deserializing profile");
            profilesCache.ProfilesSc.AddOrUpdate(deserializedProfile);
        }
    }
}
