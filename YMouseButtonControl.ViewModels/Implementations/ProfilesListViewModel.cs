using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private readonly ProcessSelectorDialogViewModel _processSelectorDialogViewModel;

    public ICommand AddButtonCommand { get; }
    public ReactiveCommand<Unit,Unit> EditButtonCommand { get; }
    public ReactiveCommand<Unit,Unit> UpCommand { get; }
    public ReactiveCommand<Unit,Unit> DownCommand { get; }
    public ReactiveCommand<Unit,Unit> RemoveButtonCommand { get; }
    public Interaction<ProcessSelectorDialogViewModel, Profile> ShowProcessSelectorInteraction { get; }

    public ProfilesListViewModel(IProfilesService profilesService, ProcessSelectorDialogViewModel processSelectorDialogViewModel)
    {
        ProfilesService = profilesService;
        AddButtonCommand = ReactiveCommand.CreateFromTask(ShowProcessPickerDialogAsync);
        var removeCanExecute = this
            .WhenAnyValue(x => x.ProfilesService.CurrentProfile, curProf => curProf.Name != "Default");
        RemoveButtonCommand = ReactiveCommand.Create(OnRemoveButtonClicked, removeCanExecute);
        var upCommandCanExecute = this
            .WhenAnyValue(x => x.ProfilesService.CurrentProfileIndex)
            .Select(x => x > 0);
        UpCommand = ReactiveCommand.Create(UpButtonClicked, upCommandCanExecute);
        var downCommandCanExecute = this
            .WhenAnyValue(x => x.ProfilesService.CurrentProfileIndex)
            .Select(x => x + 1 < ProfilesService.Profiles.Count);
        DownCommand = ReactiveCommand.Create(DownButtonClicked, downCommandCanExecute);
        _processSelectorDialogViewModel = processSelectorDialogViewModel;
        ShowProcessSelectorInteraction = new Interaction<ProcessSelectorDialogViewModel, Profile>();
        var editCanExecute = this
            .WhenAnyValue(x => x.ProfilesService.CurrentProfile, curProf => curProf.Name != "Default");
        EditButtonCommand = ReactiveCommand.CreateFromTask(EditButtonClickedAsync, editCanExecute);
    }
    
    public IProfilesService ProfilesService
    {
        get => _profilesService;
        set => _profilesService = value;
    }

    private void OnRemoveButtonClicked()
    {
        _profilesService.RemoveProfile(_profilesService.CurrentProfile);
    }

    private void UpButtonClicked()
    {
        _profilesService.MoveProfileUp(_profilesService.CurrentProfile);
    }
    
    private void DownButtonClicked()
    {
        _profilesService.MoveProfileDown(_profilesService.CurrentProfile);
    }

    private async Task EditButtonClickedAsync()
    {
        var result = await ShowProcessSelectorInteraction.Handle(_processSelectorDialogViewModel);
        if (result is null)
        {
            return;
        }

        var newProfile = _profilesService.CurrentProfile;
        newProfile.Process = result.Process;
        newProfile.Name = result.Name;
        newProfile.Description = result.Description;
        _profilesService.ReplaceProfile(_profilesService.CurrentProfile, newProfile);
    }

    private async Task ShowProcessPickerDialogAsync()
    {
        var result = await ShowProcessSelectorInteraction.Handle(_processSelectorDialogViewModel);
        if (result is null)
        {
            return;
        }
        _profilesService.AddProfile(result);
    }
}