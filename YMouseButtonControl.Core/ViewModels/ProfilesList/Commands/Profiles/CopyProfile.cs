using System;
using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class CopyProfile
{
    public sealed record Command(
        int ProfileIdToCopyBtnMappingsFrom,
        string Name,
        string Description,
        bool Checked,
        string Process,
        string MatchType,
        string ParentClass,
        string WindowClass
    );

    public sealed class Handler(IProfilesCache profilesService)
    {
        public ProfilesListProfileModel Execute(Command c)
        {
            ProfilesListProfileModel? result = default;
            profilesService.ProfilesSc.Edit(inner =>
            {
                var clonedBtnMappings = inner
                    .Items.First(x => x.Id == c.ProfileIdToCopyBtnMappingsFrom)
                    .Clone()
                    .ButtonMappings.ToList();
                var newProfileVm = new ProfileVm(clonedBtnMappings)
                {
                    Id = inner.Items.Max(x => x.Id) + 1,
                    DisplayPriority = inner.Items.Max(x => x.DisplayPriority) + 1,
                    Description = c.Description,
                    MatchType = c.MatchType,
                    ParentClass = c.ParentClass,
                    Process = c.Process,
                    WindowClass = c.WindowClass,
                    Name = c.Name,
                };

                foreach (var bm in newProfileVm.ButtonMappings)
                {
                    bm.ProfileId = newProfileVm.Id;
                }
                var maxBtnMappingId = inner.Items.SelectMany(x => x.ButtonMappings).Max(x => x.Id);
                foreach (var bm in newProfileVm.ButtonMappings)
                {
                    bm.Id = ++maxBtnMappingId;
                }
                inner.AddOrUpdate(newProfileVm);
                result = new ProfilesListProfileModel(newProfileVm, profilesService);
            });

            return result ?? throw new Exception("Result null exception");
        }
    }
}
