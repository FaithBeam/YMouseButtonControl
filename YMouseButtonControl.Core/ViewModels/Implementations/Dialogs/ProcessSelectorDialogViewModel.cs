using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public class ProcessSelectorDialogViewModel : DialogBase, IProcessSelectorDialogViewModel
{
    private ObservableCollection<ProcessModel> _processes;
    private readonly IProcessMonitorService _processMonitorService;
    private ProcessModel? _processModel;

    public ProcessSelectorDialogViewModel(IProcessMonitorService processMonitorService)
    {
        _processes = new ObservableCollection<ProcessModel>();
        _processMonitorService = processMonitorService;
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
        RefreshProcessList();
    }

    public ICommand RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, Profile> OkCommand { get; }

    public ObservableCollection<ProcessModel> Processes
    {
        get => _processes;
        private set => this.RaiseAndSetIfChanged(ref _processes, value);
    }

    public ProcessModel? SelectedProcessModel
    {
        get => _processModel;
        set => this.RaiseAndSetIfChanged(ref _processModel, value);
    }

    private void RefreshProcessList()
    {
        Processes.Clear();
        Processes.AddRange(_processMonitorService.GetProcesses.OrderBy(x => x.Process.ProcessName));
    }
}
