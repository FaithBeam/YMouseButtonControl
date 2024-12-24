using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Queries.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList;

public interface IProfilesListViewModel;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private ProfilesListProfileModel? _currentProfile;

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
        ProfileVm?
    > ShowProcessSelectorInteraction { get; }

    public ProfilesListViewModel(
        IProfilesService profilesService,
        ListProfiles.Handler listCacheProfilesHandler,
        GetCurrentProfile.Handler getCurrentProfileHandler,
        SetCurrentProfile.Handler setCurrentProfileHandler,
        RemoveProfile.Handler removeProfileHandler,
        IProcessSelectorDialogVmFactory processSelectorDialogVmFactory,
        AddProfile.Handler addProfileHandler,
        ExportProfile.Handler exportProfileHandler,
        CopyProfile.Handler copyProfileHandler
    )
    {
        _profilesService = profilesService;
        CurrentProfile = getCurrentProfileHandler.Execute();
        Profiles = listCacheProfilesHandler.Execute();
        this.WhenAnyValue(x => x.CurrentProfile)
            .WhereNotNull()
            .Select(cur => new SetCurrentProfile.Command(cur))
            .Subscribe(setCurrentProfileHandler.Execute);
        ShowProcessSelectorInteraction =
            new Interaction<IProcessSelectorDialogViewModel, ProfileVm?>();
        AddButtonCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await ShowProcessSelectorInteraction.Handle(
                processSelectorDialogVmFactory.Create()
            );
            if (result is null)
            {
                return;
            }
            addProfileHandler.Execute(new AddProfile.Command(result));
        });
        ShowExportFileDialog = new Interaction<string, string>();
        ShowImportFileDialog = new Interaction<Unit, string?>();
        ImportCommand = ReactiveCommand.CreateFromTask(OnImportClickedAsync);
        var exportCanExecute = this.WhenAnyValue(
            x => x.CurrentProfile,
            curProf => !curProf?.IsDefault ?? false
        );
        ExportCommand = ReactiveCommand.CreateFromTask(OnExportClickedAsync, exportCanExecute);

        var removeCanExecute = this.WhenAnyValue(
            x => x.CurrentProfile,
            curProf => !curProf?.IsDefault ?? false
        );
        RemoveButtonCommand = ReactiveCommand.Create(
            () =>
            {
                if (CurrentProfile is not null)
                {
                    var curProfileDisplayPriority = CurrentProfile.DisplayPriority;
                    removeProfileHandler.Execute(new RemoveProfile.Command(CurrentProfile.Id));
                    CurrentProfile = Profiles
                        .Where(x => x.DisplayPriority < curProfileDisplayPriority)
                        .MinBy(x => x.DisplayPriority);
                }
            },
            removeCanExecute
        );

        var upCommandCanExecute = this.WhenAnyValue(
            x => x.CurrentProfile,
            x => x.CurrentProfile!.DisplayPriority,
            selector: (curProf, curProfDisplayPriority) =>
                curProf is not null
                && !curProf.IsDefault
                && Profiles.Any(p => !p.IsDefault && p.DisplayPriority < curProfDisplayPriority)
        );
        UpCommand = ReactiveCommand.Create(
            () =>
            {
                var profileOfNextSmallerDisplayPriority =
                    Profiles
                        .Where(x => x.DisplayPriority < CurrentProfile!.DisplayPriority)
                        .MaxBy(x => x.DisplayPriority)
                    ?? throw new Exception("Unable to find profile to move down the profiles list");
                if (profileOfNextSmallerDisplayPriority.IsDefault)
                {
                    throw new InvalidOperationException(
                        "You cannot move the Default profile in the profiles list"
                    );
                }

                // swap display priorities
                (
                    profileOfNextSmallerDisplayPriority.DisplayPriority,
                    CurrentProfile!.DisplayPriority
                ) = (
                    CurrentProfile.DisplayPriority,
                    profileOfNextSmallerDisplayPriority.DisplayPriority
                );
            },
            upCommandCanExecute
        );
        var downCommandCanExecute = this.WhenAnyValue(
            x => x.CurrentProfile,
            x => x.CurrentProfile!.DisplayPriority,
            selector: (curProf, curProfDisplayPriority) =>
                curProf is not null
                && !curProf.IsDefault
                && Profiles.Any(p => p.DisplayPriority > curProfDisplayPriority)
        );
        DownCommand = ReactiveCommand.Create(DownButtonClicked, downCommandCanExecute);

        var editCanExecute = this.WhenAnyValue(
            x => x.CurrentProfile,
            curProf => !curProf?.IsDefault ?? false
        );
        EditButtonCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                var result = await ShowProcessSelectorInteraction.Handle(
                    processSelectorDialogVmFactory.Create(CurrentProfile?.Process)
                );
                if (result is null)
                {
                    return;
                }

                CurrentProfile!.Process = result.Process;
                CurrentProfile.Name = result.Name;
                CurrentProfile.Description = result.Description;
            },
            editCanExecute
        );
        CopyCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var newProfile = await ShowProcessSelectorInteraction.Handle(
                processSelectorDialogVmFactory.Create()
            );
            if (newProfile is null)
            {
                return;
            }

            var copiedProfile = copyProfileHandler.Execute(
                new CopyProfile.Command(
                    CurrentProfile!.Id,
                    newProfile.Name,
                    newProfile.Description,
                    newProfile.Checked,
                    newProfile.Process,
                    newProfile.MatchType,
                    newProfile.ParentClass,
                    newProfile.WindowClass
                )
            );
            setCurrentProfileHandler.Execute(new SetCurrentProfile.Command(copiedProfile));
            CurrentProfile = getCurrentProfileHandler.Execute();
        });
    }

    public Interaction<string, string> ShowExportFileDialog { get; }
    public Interaction<Unit, string?> ShowImportFileDialog { get; }

    public ProfilesListProfileModel? CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }

    public ReadOnlyObservableCollection<ProfilesListProfileModel> Profiles { get; }

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
        Debug.Assert(
            _profilesService.CurrentProfile != null,
            "_profilesService.CurrentProfile != null"
        );
        var result = await ShowExportFileDialog.Handle(_profilesService.CurrentProfile.Name);
        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }

        _profilesService.WriteProfileToFile(_profilesService.CurrentProfile, result);
    }

    private void DownButtonClicked()
    {
        Debug.Assert(
            _profilesService.CurrentProfile != null,
            "_profilesService.CurrentProfile != null"
        );
        _profilesService.MoveProfileDown(_profilesService.CurrentProfile);
    }
}
