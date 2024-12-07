using System.Linq;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

public interface IApply
{
    void ApplyProfiles();
}

public class Apply(
    YMouseButtonControlDbContext db,
    //IRepository<ButtonMapping, BaseButtonMappingVm> buttonMappingRepository,
    IProfilesService profilesService
) : IApply
{
    public void ApplyProfiles()
    {
        var dbProfiles = db.Profiles.AsNoTracking().ToList();

        // delete profiles
        dbProfiles
            .Where(x => !profilesService.Profiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x =>
            {
                var ent = db.Profiles.Find(x.Id);
                if (ent is not null)
                {
                    db.Profiles.Remove(ent);
                }
            });

        // update profiles
        profilesService
            .Profiles.Where(x => dbProfiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x =>
            {
                var ent = db.Profiles.Find(x.Id);
                if (ent is not null)
                {
                    ent.Checked = x.Checked;
                    ent.Description = x.Description;
                    ent.DisplayPriority = x.DisplayPriority;
                    ent.IsDefault = x.IsDefault;
                    ent.MatchType = x.MatchType;
                    ent.Name = x.Name;
                    ent.ParentClass = x.ParentClass;
                    ent.Process = x.Process;
                    ent.WindowCaption = x.WindowCaption;
                    ent.WindowClass = x.WindowClass;
                }
            });

        // add profiles
        profilesService
            .Profiles.Where(x => !dbProfiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x =>
            {
                var ent = ProfileMapper.Map(x);
                if (ent is not null)
                {
                    db.Profiles.Add(ent);
                }
            });

        db.SaveChanges();
    }
}
