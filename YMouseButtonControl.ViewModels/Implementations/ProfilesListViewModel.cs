using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private readonly IProcessSelectorDialogViewModel _processSelectorDialogViewModel;

    public ICommand AddButtonCommand { get; }
    public ReactiveCommand<Unit, Unit> EditButtonCommand { get; }
    public ReactiveCommand<Unit, Unit> UpCommand { get; }
    public ReactiveCommand<Unit, Unit> DownCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveButtonCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyCommand { get; }
    public ReactiveCommand<Unit, Unit> ExportCommand { get; }
    public ReactiveCommand<Unit, Unit> ImportCommand { get; }
    public Interaction<
        IProcessSelectorDialogViewModel,
        Profile?
    > ShowProcessSelectorInteraction { get; }

    public ProfilesListViewModel(
        IProfilesService profilesService,
        IProcessSelectorDialogViewModel processSelectorDialogViewModel
    )
    {
        _profilesService = profilesService;
        AddButtonCommand = ReactiveCommand.CreateFromTask(ShowProcessPickerDialogAsync);
        ShowExportFileDialog = new Interaction<string, string>();
        ShowImportFileDialog = new Interaction<Unit, string?>();
        ImportCommand = ReactiveCommand.CreateFromTask(OnImportClickedAsync);
        var exportCanExecute = this.WhenAnyValue(
            x => x.ProfilesService.CurrentProfile,
            curProf => curProf.Name != "Default"
        );
        ExportCommand = ReactiveCommand.CreateFromTask(OnExportClickedAsync, exportCanExecute);
        var removeCanExecute = this.WhenAnyValue(
            x => x.ProfilesService.CurrentProfile,
            curProf => curProf.Name != "Default"
        );
        RemoveButtonCommand = ReactiveCommand.Create(OnRemoveButtonClicked, removeCanExecute);
        var upCommandCanExecute = this.WhenAnyValue(x => x.ProfilesService.CurrentProfileIndex)
            .Select(x => x > 1);
        UpCommand = ReactiveCommand.Create(UpButtonClicked, upCommandCanExecute);
        var downCommandCanExecute = this.WhenAnyValue(x => x.ProfilesService.CurrentProfileIndex)
            .Select(x =>
                x + 1 < ProfilesService.Profiles.Count
                && ProfilesService.CurrentProfile.Name != "Default"
            );
        DownCommand = ReactiveCommand.Create(DownButtonClicked, downCommandCanExecute);
        _processSelectorDialogViewModel = processSelectorDialogViewModel;
        ShowProcessSelectorInteraction =
            new Interaction<IProcessSelectorDialogViewModel, Profile?>();
        var editCanExecute = this.WhenAnyValue(
            x => x.ProfilesService.CurrentProfile,
            curProf => curProf.Name != "Default"
        );
        EditButtonCommand = ReactiveCommand.CreateFromTask(EditButtonClickedAsync, editCanExecute);
        CopyCommand = ReactiveCommand.CreateFromTask(OnCopyClickedAsync);
    }

    public Interaction<string, string> ShowExportFileDialog { get; }
    public Interaction<Unit, string?> ShowImportFileDialog { get; }

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        set => _profilesService = value;
    }

    private async Task OnImportClickedAsync()
    {
        var result = await ShowImportFileDialog.Handle(Unit.Default);
        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }

        _profilesService.ImportProfileFromPath(result);
    }

    private async Task OnExportClickedAsync()
    {
        var result = await ShowExportFileDialog.Handle(_profilesService.CurrentProfile.Name);
        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }

        _profilesService.WriteProfileToFile(_profilesService.CurrentProfile, result);
    }

    private async Task OnCopyClickedAsync()
    {
        var newProfile = await ShowProcessSelectorInteraction.Handle(
            _processSelectorDialogViewModel
        );
        if (newProfile is null)
        {
            return;
        }

        var copiedProfile = _profilesService.CopyProfile(_profilesService.CurrentProfile);
        copiedProfile.Name = newProfile.Name;
        copiedProfile.Description = newProfile.Description;
        copiedProfile.Checked = newProfile.Checked;
        copiedProfile.Process = newProfile.Process;
        copiedProfile.MatchType = newProfile.MatchType;
        copiedProfile.ParentClass = newProfile.ParentClass;
        copiedProfile.WindowCaption = newProfile.WindowCaption;
        copiedProfile.WindowClass = newProfile.WindowClass;
        _profilesService.AddProfile(copiedProfile);
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

        var newProfile = _profilesService.CopyProfile(_profilesService.CurrentProfile);
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
