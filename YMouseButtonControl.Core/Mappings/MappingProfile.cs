using System;
using System.Collections.Generic;
using AutoMapper;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;
using Profile = YMouseButtonControl.DataAccess.Models.Profile;

namespace YMouseButtonControl.Core.Mappings;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Setting, BaseSettingVm>().IncludeAllDerived();
        CreateMap<BaseSettingVm, Setting>();
        CreateMap<SettingString, SettingStringVm>();
        CreateMap<SettingBool, SettingBoolVm>();
        CreateMap<SettingStringVm, SettingString>();
        CreateMap<SettingBoolVm, SettingBool>();
        CreateMap<SettingInt, SettingIntVm>();
        CreateMap<SettingIntVm, SettingInt>();

        CreateMap<ButtonMapping, BaseButtonMappingVm>()
            .IncludeAllDerived()
            .ForMember(
                x => x.SimulatedKeystrokeType,
                opt => opt.MapFrom<SimulatedKeystrokeTypeResolver?>()
            );
        CreateMap<BaseButtonMappingVm, ButtonMapping>()
            .IncludeAllDerived()
            .ForMember(
                x => x.SimulatedKeystrokeType,
                opt => opt.MapFrom<SimulatedKeystrokeTypeVmResolver>()
            );
        CreateMap<DisabledMapping, DisabledMappingVm>();
        CreateMap<DisabledMappingVm, DisabledMapping>();
        CreateMap<NothingMapping, NothingMappingVm>();
        CreateMap<NothingMappingVm, NothingMapping>();
        CreateMap<SimulatedKeystroke, SimulatedKeystrokeVm>();
        CreateMap<SimulatedKeystrokeVm, SimulatedKeystroke>();
        CreateMap<RightClick, RightClickVm>();
        CreateMap<RightClickVm, RightClick>();

        CreateMap<Profile, ProfileVm>()
            .ForCtorParam("buttonMappings", opt => opt.MapFrom(src => src.ButtonMappings));
        CreateMap<ProfileVm, Profile>()
            .ForMember(x => x.ButtonMappings, opt => opt.MapFrom<BaseButtonVmToButtonMapping>());
        CreateMap<ProfileVm, ProfileVm>();
    }
}

public class SimulatedKeystrokeTypeVmResolver
    : IValueResolver<BaseButtonMappingVm, ButtonMapping, SimulatedKeystrokeType?>
{
    public SimulatedKeystrokeType? Resolve(
        BaseButtonMappingVm source,
        ButtonMapping destination,
        SimulatedKeystrokeType? destMember,
        ResolutionContext context
    ) =>
        source.SimulatedKeystrokeType switch
        {
            null => null,
            AsMousePressedAndReleasedActionTypeVm asMousePressedAndReleasedActionTypeVm =>
                SimulatedKeystrokeType.AsMousePressedAndReleasedActionType,
            DuringMouseActionTypeVm duringMouseActionTypeVm =>
                SimulatedKeystrokeType.DuringMouseActionType,
            InAnotherThreadPressedActionTypeVm inAnotherThreadPressedActionTypeVm =>
                SimulatedKeystrokeType.InAnotherThreadPressedActionType,
            InAnotherThreadReleasedActionTypeVm inAnotherThreadReleasedActionTypeVm =>
                SimulatedKeystrokeType.InAnotherThreadReleasedActionType,
            MouseButtonPressedActionTypeVm mouseButtonPressedActionTypeVm =>
                SimulatedKeystrokeType.MouseButtonPressedActionType,
            MouseButtonReleasedActionTypeVm mouseButtonReleasedActionTypeVm =>
                SimulatedKeystrokeType.MouseButtonReleasedActionType,
            RepeatedlyWhileButtonDownActionTypeVm repeatedlyWhileButtonDownActionTypeVm =>
                SimulatedKeystrokeType.RepeatedlyWhileButtonDownActionType,
            StickyHoldActionTypeVm stickyHoldActionTypeVm =>
                SimulatedKeystrokeType.StickyHoldActionType,
            StickyRepeatActionTypeVm stickyRepeatActionTypeVm =>
                SimulatedKeystrokeType.StickyRepeatActionType,
            _ => throw new ArgumentOutOfRangeException(),
        };
}

public class BaseButtonVmToButtonMapping
    : IValueResolver<ProfileVm, Profile, ICollection<ButtonMapping>?>
{
    public ICollection<ButtonMapping> Resolve(
        ProfileVm source,
        Profile destination,
        ICollection<ButtonMapping>? destMember,
        ResolutionContext context
    )
    {
        var col = new List<ButtonMapping>();
        foreach (var vm in source.ButtonMappings)
        {
            switch (vm)
            {
                case DisabledMappingVm:
                    col.Add(context.Mapper.Map<DisabledMapping>(vm));
                    break;
                case NothingMappingVm:
                    col.Add(context.Mapper.Map<NothingMapping>(vm));
                    break;
                case SimulatedKeystrokeVm:
                    col.Add(context.Mapper.Map<SimulatedKeystroke>(vm));
                    break;
                case RightClickVm:
                    col.Add(context.Mapper.Map<RightClick>(vm));
                    break;
                default:
                    throw new ArgumentException($"Viewmodel is of unknown type {vm.GetType()}");
            }
        }

        return col;
    }
}

public class SimulatedKeystrokeTypeResolver
    : IValueResolver<ButtonMapping, BaseButtonMappingVm, BaseSimulatedKeystrokeTypeVm?>
{
    public BaseSimulatedKeystrokeTypeVm? Resolve(
        ButtonMapping source,
        BaseButtonMappingVm destination,
        BaseSimulatedKeystrokeTypeVm? destMember,
        ResolutionContext context
    ) =>
        source.SimulatedKeystrokeType switch
        {
            SimulatedKeystrokeType.AsMousePressedAndReleasedActionType =>
                new AsMousePressedAndReleasedActionTypeVm(),
            SimulatedKeystrokeType.DuringMouseActionType => new DuringMouseActionTypeVm(),
            SimulatedKeystrokeType.InAnotherThreadPressedActionType =>
                new InAnotherThreadPressedActionTypeVm(),
            SimulatedKeystrokeType.InAnotherThreadReleasedActionType =>
                new InAnotherThreadReleasedActionTypeVm(),
            SimulatedKeystrokeType.MouseButtonPressedActionType =>
                new MouseButtonPressedActionTypeVm(),
            SimulatedKeystrokeType.MouseButtonReleasedActionType =>
                new MouseButtonReleasedActionTypeVm(),
            SimulatedKeystrokeType.RepeatedlyWhileButtonDownActionType =>
                new RepeatedlyWhileButtonDownActionTypeVm(),
            SimulatedKeystrokeType.StickyHoldActionType => new StickyHoldActionTypeVm(),
            SimulatedKeystrokeType.StickyRepeatActionType => new StickyRepeatActionTypeVm(),
            null => null,
            _ => throw new ArgumentOutOfRangeException(),
        };
}
