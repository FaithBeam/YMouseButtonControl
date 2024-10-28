using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
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

namespace YMouseButtonControl.Headless.Tests;

public class Something : IDisposable
{
    public IConfigurationRoot Configuration { get; }
    public IConnectionProvider ConnProvider { get; }
    public YMouseButtonControlDbContext Context { get; }
    public ProfileQueries? ProfileQueries { get; set; }
    public ButtonMappingQueries? ButtonMappingQueries { get; set; }
    public SettingQueries? SettingQueries { get; set; }
    public ThemeQueries? ThemeQueries { get; set; }
    public IRepository<Profile, ProfileVm>? ProfileRepository { get; set; }
    public IRepository<Setting, BaseSettingVm>? SettingRepository { get; set; }
    public IRepository<Theme, ThemeVm>? ThemeRepository { get; set; }
    public IRepository<ButtonMapping, BaseButtonMappingVm>? ButtonMappingRepository { get; set; }
    public ProfilesService? ProfilesService { get; set; }
    public SettingsService? SettingsService { get; set; }
    public ThemeService? ThemeService { get; set; }
    public TestProvider? TestProvider { get; set; }
    public SimpleReactiveGlobalHook? Hook { get; set; }
    public ICurrentWindowService? CurrentWindowService { get; set; }
    public IMouseListener? MouseListener { get; set; }
    public IShowSimulatedKeystrokesDialogService? ShowSimulatedKeystrokesDialogService { get; set; }
    public IMouseComboViewModelFactory? MouseComboViewModelFactory { get; set; }
    public LayerViewModel? LayerViewModel { get; set; }
    public IProcessMonitorService? ProcessMonitorService { get; set; }
    public IProcessSelectorDialogViewModel? ProcessSelectorDialogViewModel { get; set; }
    public IAddProfile? AddProfile { get; set; }
    public IProfilesListViewModel? ProfilesListViewModel { get; set; }
    public ProfilesInformationViewModel? ProfilesInformationViewModel { get; set; }
    public IStartMenuInstallerService? StartMenuInstallerService { get; set; }
    public IEnableLoggingService? EnableLoggingService { get; set; }
    public GlobalSettingsDialogViewModel? GlobalSettingsDialogViewModel { get; set; }
    public IApply? ApplySvc { get; set; }
    public MainWindowViewModel? MainWindowViewModel { get; set; }
    public IMainWindowProvider? MainWindowProvider { get; set; }
    public MainWindow? MainWindow { get; set; }

    public Something()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        ConnProvider = new ConnectionProvider(Configuration);
        Context = new YMouseButtonControlDbContext(ConnProvider);
        Context.Init();
    }

    /// <summary>
    /// Add data to the database before this method
    /// </summary>
    public void Init()
    {
        ProfileQueries = new ProfileQueries();
        ButtonMappingQueries = new ButtonMappingQueries();
        SettingQueries = new SettingQueries();
        ThemeQueries = new ThemeQueries();

        ProfileRepository = new ProfileRepository(
            ConnProvider,
            ProfileQueries,
            ButtonMappingQueries
        );
        SettingRepository = new SettingRepository(ConnProvider, SettingQueries);
        ThemeRepository = new ThemeRepository(ConnProvider, ThemeQueries);
        ButtonMappingRepository = new ButtonMappingRepository(ConnProvider, ButtonMappingQueries);

        SettingsService = new SettingsService(SettingRepository);
        ProfilesService = new ProfilesService(ProfileRepository);
        ThemeService = new ThemeService(ThemeRepository, SettingsService);

        TestProvider = new TestProvider();
        Hook = new SimpleReactiveGlobalHook(TestProvider);

        CurrentWindowService = Substitute.For<ICurrentWindowService>();

        MouseListener = new MouseListener(
            NullLogger<MouseListener>.Instance,
            Hook,
            ProfilesService,
            CurrentWindowService
        );
        ShowSimulatedKeystrokesDialogService = new ShowSimulatedKeystrokesDialogService(
            MouseListener,
            ThemeService
        );
        MouseComboViewModelFactory = new MouseComboViewModelFactory(
            MouseListener,
            ThemeService,
            ProfilesService,
            ShowSimulatedKeystrokesDialogService
        );
        LayerViewModel = new LayerViewModel(MouseComboViewModelFactory, ProfilesService);
        ProcessMonitorService = Substitute.For<IProcessMonitorService>();
        ProcessSelectorDialogViewModel = new ProcessSelectorDialogViewModel(
            ProcessMonitorService,
            ThemeService
        );
        AddProfile = new AddProfile(ProfilesService);
        ProfilesListViewModel = new ProfilesListViewModel(
            ProfilesService,
            ProcessSelectorDialogViewModel,
            AddProfile
        );
        ProfilesInformationViewModel = new ProfilesInformationViewModel(ProfilesService);
        StartMenuInstallerService = Substitute.For<IStartMenuInstallerService>();
        EnableLoggingService = new EnableLoggingService();
        GlobalSettingsDialogViewModel = new GlobalSettingsDialogViewModel(
            StartMenuInstallerService,
            EnableLoggingService,
            SettingsService,
            ThemeService
        );
        ApplySvc = new Apply(ProfileRepository, ButtonMappingRepository, ProfilesService);
        MainWindowViewModel = new MainWindowViewModel(
            ProfilesService,
            ThemeService,
            LayerViewModel,
            ProfilesListViewModel,
            ProfilesInformationViewModel,
            GlobalSettingsDialogViewModel,
            ApplySvc,
            ProfileRepository
        );
        MainWindowProvider = Substitute.For<IMainWindowProvider>();
        MainWindow = new MainWindow(MainWindowProvider) { DataContext = MainWindowViewModel };
        MainWindowProvider.GetMainWindow().Returns((Window)MainWindow);
    }

    public void Dispose()
    {
        var conn = ConnProvider.CreateConnection();
        conn.Execute(
            """
            DROP TABLE Profiles;
            DROP TABLE Settings;
            DROP TABLE Themes;
            DROP TABLE ButtonMappings;
            """
        );
    }
}

public class E2eTests
{
    [AvaloniaFact]
    public void Default_profile_tests()
    {
        using var smt = new Something();
        smt.Init();
        if (smt.MainWindow is null || smt.MainWindowViewModel is null)
        {
            Assert.Fail();
        }
        smt.MainWindow.Show();

        Assert.Single(smt.MainWindow.ProfilesListView.ProfilesList.Items);
        Assert.Equal(
            "Default",
            ((ProfileVm)smt.MainWindow.ProfilesListView.ProfilesList.SelectedItem!).Name
        );
        Assert.False(smt.MainWindowViewModel.CanSave);
        Assert.True(
            smt.MainWindow.LayerView.Mb1ComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.Mb2ComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.Mb3ComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.Mb4ComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.Mb5ComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.MwuComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.MwdComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.MwlComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
        Assert.True(
            smt.MainWindow.LayerView.MwrComboVm.BtnMappingsCombo.SelectedItem is NothingMappingVm
        );
    }

    [AvaloniaFact]
    public void Add_profile()
    {
        using var smt = new Something();
        smt.Init();
        if (smt.MainWindow is null || smt.MainWindowViewModel is null)
        {
            Assert.Fail();
        }
        smt.MainWindow.Show();

        smt.MainWindow.ProfilesListView.AddBtn.Focus();
        smt.MainWindow.KeyPressQwerty(
            Avalonia.Input.PhysicalKey.Enter,
            Avalonia.Input.RawInputModifiers.None
        );
    }
}
