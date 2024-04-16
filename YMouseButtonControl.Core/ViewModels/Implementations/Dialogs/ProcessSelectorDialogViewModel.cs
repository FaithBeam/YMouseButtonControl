using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public class ProcessSelectorDialogViewModel : DialogBase, IProcessSelectorDialogViewModel
{
    private readonly IProcessMonitorService _processMonitorService;
    private ProcessModel? _processModel;
    private readonly SourceList<ProcessModel> _sourceProcessModels;
    private string? _processFilter;
    private readonly ReadOnlyObservableCollection<ProcessModel> _filtered;

    public ProcessSelectorDialogViewModel(IProcessMonitorService processMonitorService)
    {
        _sourceProcessModels = new SourceList<ProcessModel>();
        _processMonitorService = processMonitorService;
        var dynamicFilter = this.WhenValueChanged(x => x.ProcessFilter)
            .Select(CreateProcessFilterPredicate);
        var filteredDisposable = _sourceProcessModels.Connect().Filter(dynamicFilter).Bind(out _filtered).Subscribe();
        RefreshProcessList();
        RefreshButtonCommand = ReactiveCommand.Create(RefreshProcessList);
        var canExecuteOkCommand = this.WhenAnyValue(
            x => x.SelectedProcessModel,
            selector: model => model is not null
        );
        OkCommand = ReactiveCommand.Create(
            () =>
                new Profile
                {
                    Name = SelectedProcessModel!.Process.MainModule!.ModuleName,
                    Description = SelectedProcessModel.Process.MainWindowTitle,
                    Process = SelectedProcessModel.Process.MainModule.ModuleName
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


        return model => !string.IsNullOrWhiteSpace(model.Process.ProcessName) &&
                        model.Process.ProcessName.Contains(txt, StringComparison.OrdinalIgnoreCase);
    }

    public string? ProcessFilter
    {
        get => _processFilter;
        set => this.RaiseAndSetIfChanged(ref _processFilter, value);
    }

    public ICommand RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, Profile> OkCommand { get; }

    public ReadOnlyObservableCollection<ProcessModel> Filtered => _filtered;

    public ProcessModel? SelectedProcessModel
    {
        get => _processModel;
        set => this.RaiseAndSetIfChanged(ref _processModel, value);
    }

    private void RefreshProcessList()
    {
        _sourceProcessModels.Edit(x =>
        {
            x.Clear();
            x.AddRange(_processMonitorService.GetProcesses());
        });
    }
}