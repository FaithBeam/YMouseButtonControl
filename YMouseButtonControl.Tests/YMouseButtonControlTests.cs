using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using SharpHook;
using SharpHook.Reactive;
using SharpHook.Testing;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Logging;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.StartMenuInstaller;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;
using YMouseButtonControl.Core.ViewModels.ProfilesList;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Features.Add;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;
using YMouseButtonControl.Views;

namespace YMouseButtonControl.Tests;

public class YMouseButtonControlTests
{
    [AvaloniaTest]
    public void Should_have_default_profile()
    {
        // Create a window and set the view model as its data context:
        var ctx = new YMouseButtonControlDbContext(null);
        var profQueries = new ProfileQueries();
        var btnMappingQueries = new ButtonMappingQueries();
        IRepository<Profile, ProfileVm> profRepo = new ProfileRepository(
            ctx,
            profQueries,
            btnMappingQueries
        );
        var ps = new ProfilesService(profRepo);

        var settingQueries = new SettingQueries();
        IRepository<Setting, BaseSettingVm> settingRepo = new SettingRepository(
            ctx,
            settingQueries
        );
        var settingsSvc = new SettingsService(settingRepo);

        var themeQueries = new ThemeQueries();
        IRepository<Theme, ThemeVm> themeRepo = new ThemeRepository(ctx, themeQueries);
        var themeSvc = new ThemeService(themeRepo, settingsSvc);

        var testProvider = new TestProvider();
        using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
        var curWinSvcMock = Substitute.For<ICurrentWindowService>();
        var mouseListener = new MouseListener(
            NullLogger<MouseListener>.Instance,
            hook,
            ps,
            curWinSvcMock
        );

        var showSimulatedKeystrokesDialogSvc = new ShowSimulatedKeystrokesDialogService(
            mouseListener,
            themeSvc
        );

        var mbComboVmFact = new MouseComboViewModelFactory(
            mouseListener,
            themeSvc,
            ps,
            showSimulatedKeystrokesDialogSvc
        );
        var layerVm = new LayerViewModel(mbComboVmFact, ps);

        var procMonSvc = Substitute.For<IProcessMonitorService>();
        var processSelectorDialogVm = new ProcessSelectorDialogViewModel(procMonSvc, themeSvc);
        var addProfSvc = new AddProfile(ps);
        var profilesListVm = new ProfilesListViewModel(ps, processSelectorDialogVm, addProfSvc);

        var profsInfoVm = new ProfilesInformationViewModel(ps);
        var startMenuInstallerSvcMock = Substitute.For<IStartMenuInstallerService>();
        var enableLoggingSvc = new EnableLoggingService();
        var globalSettingsVm = new GlobalSettingsDialogViewModel(
            startMenuInstallerSvcMock,
            enableLoggingSvc,
            settingsSvc,
            themeSvc
        );
        IRepository<ButtonMapping, BaseButtonMappingVm> btnMappingRepo =
            new ButtonMappingRepository(ctx, btnMappingQueries);
        var applySvc = new Apply(profRepo, btnMappingRepo, ps);
        var window = new MainWindow
        {
            DataContext = new MainWindowViewModel(
                ps,
                themeSvc,
                layerVm,
                profilesListVm,
                profsInfoVm,
                globalSettingsVm,
                applySvc,
                profRepo
            ),
        };

        // Show the window, as it's required to get layout processed:
        window.Show();
        ;
        //window.ProfilesListView.ProfilesList.ItemsSource.

        //// Set values to the input boxes by simulating text input:
        //window.FirstOperandInput.Focus();
        //window.KeyTextInput("10");

        //// Or directly to the control:
        //window.SecondOperandInput.Text = "20";

        //// Raise click event on the button:
        //window.AddButton.Focus();
        //window.KeyPress(Key.Enter, RawInputModifiers.None);

        //Assert.That(window.ResultBox.Text, Is.EqualTo("30"));
    }
}
