using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

public class Apply(YMouseButtonControlDbContext db, IProfilesService profilesService) : IApply
{
    public void ApplyProfiles()
    {
        var dbProfiles = db.Profiles.AsNoTracking().ToList();

        // delete profiles that exist in the db but not in the profiles service. User had to remove a profile for this to occur
        dbProfiles
            .Where(x => profilesService.Profiles.All(y => y.Id != x.Id))
            .ToList()
            .ForEach(x =>
            {
                var ent = db.Profiles.Find(x.Id);
                if (ent is not null)
                {
                    db.Profiles.Remove(ent);
                }
            });

        // update profiles that exist in both profiles service and db
        profilesService
            .Profiles.Where(x => dbProfiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(profilesServicePvm =>
            {
                var ent = db.Profiles.Find(profilesServicePvm.Id);
                if (ent is null)
                {
                    throw new Exception("Profile not found");
                }
                ent.Checked = profilesServicePvm.Checked;
                ent.Description = profilesServicePvm.Description;
                ent.DisplayPriority = profilesServicePvm.DisplayPriority;
                ent.IsDefault = profilesServicePvm.IsDefault;
                ent.MatchType = profilesServicePvm.MatchType;
                ent.Name = profilesServicePvm.Name;
                ent.ParentClass = profilesServicePvm.ParentClass;
                ent.Process = profilesServicePvm.Process;
                ent.WindowCaption = profilesServicePvm.WindowCaption;
                ent.WindowClass = profilesServicePvm.WindowClass;

                foreach (var profilesServicePvmBtnMapVm in profilesServicePvm.ButtonMappings)
                {
                    var dbBm = db.ButtonMappings.Find(profilesServicePvmBtnMapVm.Id);

                    // if button mapping doesn't exist in the db, add it
                    // else if button mapping exists in the db and does not equal the button mapping in the profiles service profile, update button mapping
                    if (dbBm is null)
                    {
                        var newBm = ButtonMappingMapper.Map(profilesServicePvmBtnMapVm);
                        db.ButtonMappings.Add(newBm);
                    }
                    else
                    {
                        var dbBmMapped = ButtonMappingMapper.Map(dbBm);
                        if (profilesServicePvmBtnMapVm.Equals(dbBmMapped))
                        {
                            continue;
                        }

                        dbBm.AutoRepeatDelay = profilesServicePvmBtnMapVm.AutoRepeatDelay;
                        dbBm.AutoRepeatRandomizeDelayEnabled =
                            profilesServicePvmBtnMapVm.AutoRepeatRandomizeDelayEnabled;
                        dbBm.BlockOriginalMouseInput =
                            profilesServicePvmBtnMapVm.BlockOriginalMouseInput;
                        dbBm.Keys = profilesServicePvmBtnMapVm.Keys;
                        dbBm.MouseButton = profilesServicePvmBtnMapVm.MouseButton;
                        dbBm.Selected = profilesServicePvmBtnMapVm.Selected;
                        if (profilesServicePvmBtnMapVm.SimulatedKeystrokeType is not null)
                        {
                            dbBm.SimulatedKeystrokeType =
                                profilesServicePvmBtnMapVm.SimulatedKeystrokeType switch
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
                }
            });

        // add profiles that exist in the profile service but not the db. this occurs when a user adds a profile
        profilesService
            .Profiles.Where(x => dbProfiles.All(y => y.Id != x.Id))
            .ToList()
            .ForEach(x =>
            {
                var ent = ProfileMapper.Map(x);
                db.Profiles.Add(ent);
            });

        db.SaveChanges();
    }
}
