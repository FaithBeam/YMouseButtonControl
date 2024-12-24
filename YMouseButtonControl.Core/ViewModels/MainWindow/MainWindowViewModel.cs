﻿using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;
using YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;
using YMouseButtonControl.Core.ViewModels.ProfilesList;
using YMouseButtonControl.Domain.Models;

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
    IThemeService ThemeService { get; }
}

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private readonly IThemeService _themeService;
    private readonly IProfilesListViewModel _profilesListViewModel;
    private readonly IGlobalSettingsDialogViewModel _globalSettingsDialogViewModel;
    private bool _dirty;
    private bool Dirty
    {
        get => _dirty;
        set => this.RaiseAndSetIfChanged(ref _dirty, value);
    }

    #endregion

    #region Constructor

    public MainWindowViewModel(
        IProfilesCache pc,
        IThemeService themeService,
        ILayerViewModel layerViewModel,
        IProfilesListViewModel profilesListViewModel,
        IProfilesInformationViewModel profilesInformationViewModel,
        IGlobalSettingsDialogViewModel globalSettingsDialogViewModel,
        IApply apply,
        IsCacheDirty.Handler isCacheDirtyHandler
    )
    {
        _profilesListViewModel = profilesListViewModel;
        _globalSettingsDialogViewModel = globalSettingsDialogViewModel;
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
        var myOp = pc.Connect()
            .AutoRefresh()
            .DisposeMany()
            .Subscribe((_) => Dirty = isCacheDirtyHandler.Execute());
        var canExecuteApply = this.WhenAnyValue(x => x.Dirty);
        ApplyCommand = ReactiveCommand.Create(apply.ApplyProfiles, canExecuteApply);
        var isExecutingObservable = this.WhenAnyObservable(x => x.ApplyCommand.IsExecuting);
        canExecuteApply = canExecuteApply.Merge(isExecutingObservable);
        isExecutingObservable.Skip(1).Where(x => !x).Subscribe(x => Dirty = false);
    }

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
