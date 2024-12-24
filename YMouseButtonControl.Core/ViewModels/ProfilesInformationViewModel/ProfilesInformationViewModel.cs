using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;

public interface IProfilesInformationViewModel { }

public class ProfilesInformationViewModel : ViewModelBase, IProfilesInformationViewModel
{
    private IProfilesCache ProfilesCache { get; }
    private readonly ObservableAsPropertyHelper<ProfilesInformationModel> _profileInfo;
    public ProfilesInformationModel ProfileInfo => _profileInfo.Value;

    public ProfilesInformationViewModel(IProfilesCache profilesCache)
    {
        ProfilesCache = profilesCache;
        _profileInfo = this.WhenAnyValue(
                x => x.ProfilesCache.CurrentProfile,
                selector: vm => new ProfilesInformationModel(vm)
            )
            .ToProperty(this, x => x.ProfileInfo);
    }
}

public class ProfilesInformationModel(Models.ProfileVm? vm)
{
    public string Description { get; } = vm?.Description ?? "N/A";
    public string WindowCaption { get; } = vm?.WindowCaption ?? "N/A";
    public string Process { get; } = vm?.Process ?? "N/A";
    public string WindowClass { get; } = vm?.WindowClass ?? "N/A";
    public string ParentClass { get; } = vm?.ParentClass ?? "N/A";
    public string MatchType { get; } = vm?.MatchType ?? "N/A";
}
