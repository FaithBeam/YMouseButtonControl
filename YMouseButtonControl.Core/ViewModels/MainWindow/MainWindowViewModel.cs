using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;
using YMouseButtonControl.Core.ViewModels.ProfilesList;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IMainWindowViewModel
{
    IProfilesInformationViewModel ProfilesInformationViewModel { get; }
    IProfilesListViewModel ProfilesListViewModel { get; }
    ILayerViewModel LayerViewModel { get; }
    ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    ReactiveCommand<Unit, Unit> CloseCommand { get; }
    ReactiveCommand<Unit, Unit> SettingsCommand { get; }
    Interaction<IGlobalSettingsDialogViewModel, Unit> ShowSettingsDialogInteraction { get; }
    ProfileVm? CurrentProfile { get; }
    IThemeService ThemeService { get; }
}

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private readonly IProfilesService _ps;
    private readonly IThemeService _themeService;
    private readonly IProfilesListViewModel _profilesListViewModel;
    private readonly IGlobalSettingsDialogViewModel _globalSettingsDialogViewModel;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ObservableAsPropertyHelper<bool>? _isExecutingSave;
    private readonly ReadOnlyObservableCollection<ProfileVm> _profileVms;
    private bool _canSave;

    #endregion

    #region Constructor

    public MainWindowViewModel(
        IProfilesService ps,
        IThemeService themeService,
        ILayerViewModel layerViewModel,
        IProfilesListViewModel profilesListViewModel,
        IProfilesInformationViewModel profilesInformationViewModel,
        IGlobalSettingsDialogViewModel globalSettingsDialogViewModel,
        IApply apply,
        IUnitOfWork unitOfWork
    )
    {
        _profilesListViewModel = profilesListViewModel;
        _globalSettingsDialogViewModel = globalSettingsDialogViewModel;
        _unitOfWork = unitOfWork;
        _ps = ps;
        _themeService = themeService;
        LayerViewModel = layerViewModel;
        ProfilesInformationViewModel = profilesInformationViewModel;
        SettingsCommand = ReactiveCommand.CreateFromTask(ShowSettingsDialogAsync);
        ShowSettingsDialogInteraction = new Interaction<IGlobalSettingsDialogViewModel, Unit>();
        CloseCommand = ReactiveCommand.Create(() =>
        {
            if (
                Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime lifetime
            )
            {
                lifetime.MainWindow?.Hide();
            }
        });
        var myOp = _ps.Connect()
            .AutoRefresh()
            .RefCount()
            .Bind(out _profileVms)
            .DisposeMany()
            .Subscribe(CanSaveHelper);
        var isExecutingObservable = this.WhenAnyValue(x => x.IsExecutingSave)
            .Subscribe(_ => CanSave = false);
        var canSaveCmd = this.WhenAnyValue(x => x.CanSave);
        ApplyCommand = ReactiveCommand.Create(apply.ApplyProfiles, canSaveCmd);
        _isExecutingSave = ApplyCommand.IsExecuting.ToProperty(this, x => x.IsExecutingSave);
    }

    private void CanSaveHelper(IChangeSet<ProfileVm, int> changeSet)
    {
        foreach (var cs in changeSet)
        {
            var entity = _unitOfWork
                .ProfileRepo.Get(x => x.Id == cs.Current.Id, includeProperties: "ButtonMappings")
                .FirstOrDefault();
            switch (cs.Reason)
            {
                case ChangeReason.Add:
                    CanSave = entity is null;
                    break;
                case ChangeReason.Update:
                    CanSave = !entity?.Equals(cs.Current) ?? true;
                    break;
                case ChangeReason.Remove:
                    CanSave = entity is not null;
                    break;
                case ChangeReason.Refresh:
                    CanSave = !entity?.Equals(cs.Current) ?? true;
                    break;
                case ChangeReason.Moved:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public ProfileVm? CurrentProfile => _ps.CurrentProfile;

    public bool CanSave
    {
        get => _canSave;
        set => this.RaiseAndSetIfChanged(ref _canSave, value);
    }
    public bool IsExecutingSave => _isExecutingSave?.Value ?? false;

    #endregion

    #region Properties

    public IProfilesInformationViewModel ProfilesInformationViewModel { get; }

    public IProfilesListViewModel ProfilesListViewModel => _profilesListViewModel;

    public ILayerViewModel LayerViewModel { get; }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    public ReactiveCommand<Unit, Unit> SettingsCommand { get; }

    public Interaction<IGlobalSettingsDialogViewModel, Unit> ShowSettingsDialogInteraction { get; }

    public IThemeService ThemeService => _themeService;

    #endregion

    private async Task ShowSettingsDialogAsync()
    {
        await ShowSettingsDialogInteraction.Handle(_globalSettingsDialogViewModel);
    }
}
