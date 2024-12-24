using System.IO;
using System.Linq;
using Newtonsoft.Json;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class ExportProfile
{
    public sealed record Command(int Id, string Path);

    public sealed class Handler(IProfilesCache profilesService)
    {
        public void Execute(Command c)
        {
            var profileToExport = profilesService.Profiles.First(x => x.Id == c.Id);
            var jsonString = JsonConvert.SerializeObject(
                profileToExport,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                }
            );
            File.WriteAllText(c.Path, jsonString);
        }
    }
}
