using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Services.Profiles.Queries.Profiles;

public static class ListDbProfiles
{
    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public List<ProfileVm> Execute() =>
            db
                .Profiles.AsNoTracking()
                .Include(x => x.ButtonMappings)
                .ToList()
                .Select(ProfileMapper.Map)
                .ToList();
    }
}
