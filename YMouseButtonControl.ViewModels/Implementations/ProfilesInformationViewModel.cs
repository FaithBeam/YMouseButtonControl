using ReactiveUI;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesInformationViewModel : ViewModelBase, IProfilesInformationViewModel
{
    private readonly IProfileOperationsMediator _profileOperationsMediator;

    public ProfilesInformationViewModel(IProfileOperationsMediator profileOperationsMediator)
    {
        _profileOperationsMediator = profileOperationsMediator;
        _profileOperationsMediator.SelectedProfileChanged += OnSelectedProfileChanged;
    }

    public string Description => _profileOperationsMediator.CurrentProfile.Description;

    public string WindowCaption => _profileOperationsMediator.CurrentProfile.WindowCaption;

    public string Process => _profileOperationsMediator.CurrentProfile.Process;

    public string WindowClass => _profileOperationsMediator.CurrentProfile.WindowClass;

    public string ParentClass => _profileOperationsMediator.CurrentProfile.ParentClass;

    public string MatchType => _profileOperationsMediator.CurrentProfile.MatchType;

    private void OnSelectedProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        this.RaisePropertyChanged(nameof(Description));
        this.RaisePropertyChanged(nameof(WindowCaption));
        this.RaisePropertyChanged(nameof(Process));
        this.RaisePropertyChanged(nameof(WindowClass));
        this.RaisePropertyChanged(nameof(ParentClass));
        this.RaisePropertyChanged(nameof(MatchType));
    }
}