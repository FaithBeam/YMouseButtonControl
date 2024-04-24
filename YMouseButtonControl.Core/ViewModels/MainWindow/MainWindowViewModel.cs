using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.ViewModels.Implementations;
using YMouseButtonControl.Core.ViewModels.Interfaces;
using YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IMainWindowViewModel { }

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private readonly IProfilesService _ps;
    private readonly IProfilesListViewModel _profilesListViewModel;
    private string? _profileName;

    #endregion

    #region Constructor

    public MainWindowViewModel(
        IProfilesService ps,
        ILayerViewModel layerViewModel,
        IProfilesListViewModel profilesListViewModel,
        IProfilesInformationViewModel profilesInformationViewModel,
        IApply apply
    )
    {
        _profilesListViewModel = profilesListViewModel;
        _ps = ps;
        LayerViewModel = layerViewModel;
        ProfilesInformationViewModel = profilesInformationViewModel;
        var canApply = this.WhenAnyValue(x => x._ps.UnsavedChanges).DistinctUntilChanged();
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
        ApplyCommand = ReactiveCommand.Create(apply.ApplyProfiles, canApply);
        ApplyCommand.Subscribe(_ =>
        {
            _ps.UnsavedChanges = false;
        });
        this.WhenAnyValue(x => x._ps.CurrentProfile).WhereNotNull().Subscribe(OnProfileChanged);
        Debug.Assert(_ps.CurrentProfile != null, "_ps.CurrentProfile != null");
        ProfileName = _ps.CurrentProfile.Name;
    }

    #endregion

    #region Properties

    public IProfilesInformationViewModel ProfilesInformationViewModel { get; }

    public IProfilesListViewModel ProfilesListViewModel => _profilesListViewModel;

    public ILayerViewModel LayerViewModel { get; }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    public string? ProfileName
    {
        get => _profileName;
        set => this.RaiseAndSetIfChanged(ref _profileName, value);
    }

    #endregion

    private void OnProfileChanged(Profile profile) => ProfileName = profile.Name;
}
