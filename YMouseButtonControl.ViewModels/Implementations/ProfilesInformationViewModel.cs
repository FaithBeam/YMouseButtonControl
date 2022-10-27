using ReactiveUI;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesInformationViewModel : ViewModelBase, IProfilesInformationViewModel
{
    private readonly ICurrentProfileOperationsMediator _currentProfileOperationsMediator;

    public ProfilesInformationViewModel(ICurrentProfileOperationsMediator currentProfileOperationsMediator)
    {
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
        _currentProfileOperationsMediator.CurrentProfileChanged += OnSelectedCurrentProfileChanged;
    }

    public string Description => _currentProfileOperationsMediator.CurrentProfile.Description;

    public string WindowCaption => _currentProfileOperationsMediator.CurrentProfile.WindowCaption;

    public string Process => _currentProfileOperationsMediator.CurrentProfile.Process;

    public string WindowClass => _currentProfileOperationsMediator.CurrentProfile.WindowClass;

    public string ParentClass => _currentProfileOperationsMediator.CurrentProfile.ParentClass;

    public string MatchType => _currentProfileOperationsMediator.CurrentProfile.MatchType;

    private void OnSelectedCurrentProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        this.RaisePropertyChanged(nameof(Description));
        this.RaisePropertyChanged(nameof(WindowCaption));
        this.RaisePropertyChanged(nameof(Process));
        this.RaisePropertyChanged(nameof(WindowClass));
        this.RaisePropertyChanged(nameof(ParentClass));
        this.RaisePropertyChanged(nameof(MatchType));
    }
}