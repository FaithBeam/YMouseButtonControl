using System.IO;
using System.Linq;
using Newtonsoft.Json;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class ExportProfile
{
    public sealed record Command(int Id, string Path);

    public sealed class Handler(IProfilesCache profilesCache, ProfileVmConverter profileVmConverter)
    {
        public void Execute(Command c)
        {
            var profileToExport = profilesCache.Profiles.First(x => x.Id == c.Id);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
            };
            settings.Converters.Add(profileVmConverter);
            var jsonString = JsonConvert.SerializeObject(profileToExport, settings);
            File.WriteAllText(c.Path, jsonString);
        }
    }
}
