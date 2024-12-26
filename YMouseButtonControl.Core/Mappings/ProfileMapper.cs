using System.Linq;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Mappings;

public static class ProfileMapper
{
    public static ProfileVm MapToViewModel(Profile profile)
    {
        var buttonMappings = profile
            .ButtonMappings.Select(ButtonMappingMapper.MapToViewModel)
            .ToList();
        var viewModel = new ProfileVm(buttonMappings)
        {
            Id = profile.Id,
            IsDefault = profile.IsDefault,
            Checked = profile.Checked,
            Name = profile.Name,
            Description = profile.Description,
            WindowCaption = profile.WindowCaption,
            Process = profile.Process,
            WindowClass = profile.WindowClass,
            ParentClass = profile.ParentClass,
            MatchType = profile.MatchType,
            DisplayPriority = profile.DisplayPriority,
        };

        return viewModel;
    }

    public static Profile MapToEntity(ProfileVm profileVm) =>
        new()
        {
            Id = profileVm.Id,
            IsDefault = profileVm.IsDefault,
            Checked = profileVm.Checked,
            Name = profileVm.Name,
            Description = profileVm.Description,
            WindowCaption = profileVm.WindowCaption,
            Process = profileVm.Process,
            WindowClass = profileVm.WindowClass,
            ParentClass = profileVm.ParentClass,
            MatchType = profileVm.MatchType,
            DisplayPriority = profileVm.DisplayPriority,
            ButtonMappings = profileVm
                .ButtonMappings.Select(ButtonMappingMapper.MapToEntity)
                .ToList(),
        };
}
