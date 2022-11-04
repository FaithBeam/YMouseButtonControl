using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private ICurrentProfileOperationsMediator _currentProfileOperationsMediator;
    private ProcessSelectorDialogViewModel _processSelectorDialogViewModel;

    public ICommand AddButtonCommand { get; }
    public Interaction<ProcessSelectorDialogViewModel, Profile> ShowProcessSelectorInteraction { get; }

    public ProfilesListViewModel(IProfilesService profilesService, ICurrentProfileOperationsMediator currentProfileOperationsMediator, ProcessSelectorDialogViewModel processSelectorDialogViewModel)
    {
        ProfilesService = profilesService;
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
        _currentProfileOperationsMediator.CurrentProfile = _profilesService.GetProfiles().FirstOrDefault();
        AddButtonCommand = ReactiveCommand.CreateFromTask(ShowProcessPickerDialogAsync);
        _processSelectorDialogViewModel = processSelectorDialogViewModel;
        ShowProcessSelectorInteraction = new Interaction<ProcessSelectorDialogViewModel, Profile>();
    }

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        set => _profilesService = value;
    }

    public AvaloniaList<Profile> Profiles => new(_profilesService.GetProfiles());

    public Profile SelectedProfile
    {
        get => _currentProfileOperationsMediator.CurrentProfile;
        set => _currentProfileOperationsMediator.CurrentProfile = value;
    }

    private async Task ShowProcessPickerDialogAsync()
    {
        var result = await ShowProcessSelectorInteraction.Handle(_processSelectorDialogViewModel);
        if (result is null)
        {
            return;
        }
        _profilesService.AddProfile(result);
        this.RaisePropertyChanged(nameof(Profiles));
    }
}