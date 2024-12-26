using System;
using System.Linq;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Profiles;

public static class IsCacheDirty
{
    public sealed class Handler(IProfilesCache profilesCache, YMouseButtonControlDbContext db)
    {
        public bool Execute() =>
            !profilesCache.Profiles.SequenceEqual(db.Profiles.Select(ProfileMapper.MapToViewModel));
    }
}
