using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesInformationViewModel : ViewModelBase, IProfilesInformationViewModel
{
    private readonly IProfilesService _profilesService;

    public ProfilesInformationViewModel(IProfilesService profilesService)
    {
        _profilesService = profilesService;
    }

    public string Description => _profilesService.CurrentProfile.Description;

    public string WindowCaption => _profilesService.CurrentProfile.WindowCaption;

    public string Process => _profilesService.CurrentProfile.Process;

    public string WindowClass => _profilesService.CurrentProfile.WindowClass;

    public string ParentClass => _profilesService.CurrentProfile.ParentClass;

    public string MatchType => _profilesService.CurrentProfile.MatchType;
}