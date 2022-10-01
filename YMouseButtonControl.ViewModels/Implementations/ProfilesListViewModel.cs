using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private IProfileOperationsMediator _profileOperationsMediator;
    private ProcessSelectorDialogViewModel _processSelectorDialogViewModel;

    public ICommand AddButtonCommand { get; }
    public Interaction<ProcessSelectorDialogViewModel, ProcessModel> ShowProcessSelectorInteraction { get; }

    public ProfilesListViewModel(IProfilesService profilesService, IProfileOperationsMediator profileOperationsMediator, ProcessSelectorDialogViewModel processSelectorDialogViewModel)
    {
        ProfilesService = profilesService;
        _profileOperationsMediator = profileOperationsMediator;
        AddButtonCommand = ReactiveCommand.CreateFromTask(ShowProcessPickerDialogAsync);
        _processSelectorDialogViewModel = processSelectorDialogViewModel;
        ShowProcessSelectorInteraction = new Interaction<ProcessSelectorDialogViewModel, ProcessModel>();
    }

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        set => _profilesService = value;
    }

    public AvaloniaList<Profile> Profiles => new(_profilesService.GetProfiles());

    public Profile SelectedProfile
    {
        get => _profileOperationsMediator.CurrentProfile;
        set => _profileOperationsMediator.CurrentProfile = value;
    }

    private async Task ShowProcessPickerDialogAsync()
    {
        var result = await ShowProcessSelectorInteraction.Handle(_processSelectorDialogViewModel);
    }
}