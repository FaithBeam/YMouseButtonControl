using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
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

                    x.ButtonMappings.ForEach(bm =>
                    {
                        var dbBm = db.ButtonMappings.Find(bm.Id);
                        if (dbBm is not null)
                        {
                            dbBm.AutoRepeatDelay = bm.AutoRepeatDelay;
                            dbBm.AutoRepeatRandomizeDelayEnabled =
                                bm.AutoRepeatRandomizeDelayEnabled;
                            dbBm.BlockOriginalMouseInput = bm.BlockOriginalMouseInput;
                            dbBm.ButtonMappingType = bm switch
                            {
                                DisabledMappingVm => ButtonMappingType.Disabled,
                                NothingMappingVm => ButtonMappingType.Nothing,
                                SimulatedKeystrokeVm => ButtonMappingType.SimulatedKeystroke,
                                RightClickVm => ButtonMappingType.RightClick,
                                _ => throw new System.NotImplementedException(),
                            };
                            dbBm.Keys = bm.Keys;
                            dbBm.MouseButton = bm.MouseButton;
                            dbBm.Selected = bm.Selected;
                            if (bm.SimulatedKeystrokeType is not null)
                            {
                                dbBm.SimulatedKeystrokeType = bm.SimulatedKeystrokeType switch
                                {
                                    AsMousePressedAndReleasedActionTypeVm =>
                                        SimulatedKeystrokeType.AsMousePressedAndReleasedActionType,
                                    DuringMouseActionTypeVm =>
                                        SimulatedKeystrokeType.DuringMouseActionType,
                                    InAnotherThreadPressedActionTypeVm =>
                                        SimulatedKeystrokeType.InAnotherThreadPressedActionType,
                                    InAnotherThreadReleasedActionTypeVm =>
                                        SimulatedKeystrokeType.InAnotherThreadReleasedActionType,
                                    MouseButtonPressedActionTypeVm =>
                                        SimulatedKeystrokeType.MouseButtonPressedActionType,
                                    MouseButtonReleasedActionTypeVm =>
                                        SimulatedKeystrokeType.MouseButtonReleasedActionType,
                                    RepeatedlyWhileButtonDownActionTypeVm =>
                                        SimulatedKeystrokeType.RepeatedlyWhileButtonDownActionType,
                                    StickyHoldActionTypeVm =>
                                        SimulatedKeystrokeType.StickyHoldActionType,
                                    StickyRepeatActionTypeVm =>
                                        SimulatedKeystrokeType.StickyRepeatActionType,
                                    _ => throw new System.NotImplementedException(),
                                };
                            }
                        }
                    });
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
