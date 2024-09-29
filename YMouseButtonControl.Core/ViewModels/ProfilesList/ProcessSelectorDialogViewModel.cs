using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

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
        IThemeService themeService
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
            .RefCount()
            .Bind(out _filtered)
            .DisposeMany()
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
                var mb1 = new NothingMappingVm { MouseButton = MouseButton.Mb1 };
                var mb2 = new NothingMappingVm { MouseButton = MouseButton.Mb2 };
                var mb3 = new NothingMappingVm { MouseButton = MouseButton.Mb3 };
                var mb4 = new NothingMappingVm { MouseButton = MouseButton.Mb4 };
                var mb5 = new NothingMappingVm { MouseButton = MouseButton.Mb5 };
                var mwu = new NothingMappingVm { MouseButton = MouseButton.Mwu };
                var mwd = new NothingMappingVm { MouseButton = MouseButton.Mwd };
                var mwl = new NothingMappingVm { MouseButton = MouseButton.Mwl };
                var mwr = new NothingMappingVm { MouseButton = MouseButton.Mwr };

                var buttonMappings = new List<BaseButtonMappingVm>()
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
