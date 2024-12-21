using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList;

public interface IProcessSelectorDialogViewModel
{
    ICommand RefreshButtonCommand { get; }
}

public class ProcessSelectorDialogViewModel : DialogBase, IProcessSelectorDialogViewModel
{
    private readonly IProcessMonitorService _processMonitorService;
    private readonly SourceList<ProcessModel> _sourceProcessModels;
    private string? _processFilter;
    private readonly ReadOnlyObservableCollection<ProcessModel> _filtered;
    private ProcessModel? _processModel;

    public ProcessSelectorDialogViewModel(
        IProcessMonitorService processMonitorService,
        IThemeService themeService,
        IProfilesService profilesService
    )
        : base(themeService)
    {
        _sourceProcessModels = new SourceList<ProcessModel>();
        _processMonitorService = processMonitorService;
        var dynamicFilter = this.WhenValueChanged(x => x.ProcessFilter)
            .Select(CreateProcessFilterPredicate);
        var filteredDisposable = _sourceProcessModels
            .Connect()
            .Filter(dynamicFilter)
            .Bind(out _filtered)
            .Subscribe();
        RefreshProcessList();
        RefreshButtonCommand = ReactiveCommand.Create(RefreshProcessList);
        var canExecuteOkCommand = this.WhenAnyValue(
            x => x.SelectedProcessModel,
            selector: model => model is not null
        );
        OkCommand = ReactiveCommand.Create(
            () =>
            {
                var maxBmId = profilesService
                    .Profiles.SelectMany(x => x.ButtonMappings)
                    .Max(x => x.Id);
                var mb1 = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mb1,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mb1, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mb1, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mb1, Id = ++maxBmId },
                };

                var mb2 = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mb2,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mb2, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mb2, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mb2, Id = ++maxBmId },
                };

                var mb3 = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mb3,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mb3, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mb3, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mb3, Id = ++maxBmId },
                };

                var mb4 = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mb4,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mb4, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mb4, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mb4, Id = ++maxBmId },
                };

                var mb5 = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mb5,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mb5, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mb5, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mb5, Id = ++maxBmId },
                };

                var mwu = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mwu,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mwu, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mwu, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mwu, Id = ++maxBmId },
                };

                var mwd = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mwd,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mwd, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mwd, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mwd, Id = ++maxBmId },
                };

                var mwl = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mwl,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mwl, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mwl, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mwl, Id = ++maxBmId },
                };

                var mwr = new List<BaseButtonMappingVm>
                {
                    new NothingMappingVm
                    {
                        MouseButton = MouseButton.Mwr,
                        Selected = true,
                        Id = ++maxBmId,
                    },
                    new DisabledMappingVm { MouseButton = MouseButton.Mwr, Id = ++maxBmId },
                    new SimulatedKeystrokeVm { MouseButton = MouseButton.Mwr, Id = ++maxBmId },
                    new RightClickVm { MouseButton = MouseButton.Mwr, Id = ++maxBmId },
                };

                var buttonMappings = new List<BaseButtonMappingVm>
                {
                    mb1,
                    mb2,
                    mb3,
                    mb4,
                    mb5,
                    mwu,
                    mwd,
                    mwl,
                    mwr,
                };

                return new ProfileVm(buttonMappings)
                {
                    Name = SelectedProcessModel!.Process.MainModule!.ModuleName,
                    Description = SelectedProcessModel.Process.MainWindowTitle,
                    Process = SelectedProcessModel.Process.MainModule.ModuleName,
                    WindowCaption = "N/A",
                    WindowClass = "N/A",
                    ParentClass = "N/A",
                    MatchType = "N/A",
                };
            },
            canExecuteOkCommand
        );
    }

    private Func<ProcessModel, bool> CreateProcessFilterPredicate(string? txt)
    {
        if (string.IsNullOrWhiteSpace(txt))
        {
            return _ => true;
        }

        return model =>
            !string.IsNullOrWhiteSpace(model.Process.ProcessName)
            && model.Process.ProcessName.Contains(txt, StringComparison.OrdinalIgnoreCase);
    }

    public string? ProcessFilter
    {
        get => _processFilter;
        set => this.RaiseAndSetIfChanged(ref _processFilter, value);
    }

    public ICommand RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, ProfileVm> OkCommand { get; }

    public ReadOnlyObservableCollection<ProcessModel> Filtered => _filtered;

    public ProcessModel? SelectedProcessModel
    {
        get => _processModel;
        set => this.RaiseAndSetIfChanged(ref _processModel, value);
    }

    private void RefreshProcessList() =>
        _sourceProcessModels.EditDiff(_processMonitorService.GetProcesses());
}
