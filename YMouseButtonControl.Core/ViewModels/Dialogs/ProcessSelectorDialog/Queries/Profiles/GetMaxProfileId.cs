﻿using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Profiles;

public static class GetMaxProfileId
{
    public sealed class Handler(IProfilesCache profilesService)
    {
        public int Execute() =>
            profilesService.Profiles.SelectMany(x => x.ButtonMappings).Max(x => x.Id);
    }
}
