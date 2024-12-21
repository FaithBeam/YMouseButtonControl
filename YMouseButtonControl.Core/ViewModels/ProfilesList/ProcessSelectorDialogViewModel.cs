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
    ReactiveCommand<Unit, Unit> RefreshButtonCommand { get; }
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
            .Sort(SortExpressionComparer<ProcessModel>.Ascending(x => x.Process.ProcessName))
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
                var buttonMappings = new List<BaseButtonMappingVm>
                {
                    CreateButtonMappings(maxBmId, MouseButton.Mb1),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb2),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb3),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb4),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb5),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwu),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwd),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwl),
                    CreateButtonMappings(maxBmId + 4, MouseButton.Mwr),
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

    private static List<BaseButtonMappingVm> CreateButtonMappings(
        int id,
        MouseButton mouseButton
    ) =>
        [
            new NothingMappingVm
            {
                Id = ++id,
                MouseButton = mouseButton,
                Selected = true,
            },
            new DisabledMappingVm { Id = ++id, MouseButton = mouseButton },
            new SimulatedKeystrokeVm { Id = ++id, MouseButton = mouseButton },
            new RightClickVm { Id = ++id, MouseButton = mouseButton },
        ];

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

    public ReactiveCommand<Unit, Unit> RefreshButtonCommand { get; }

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
