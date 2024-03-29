#nullable enable
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Implementations.Dialogs;

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
        OkCommand = ReactiveCommand.Create(
            () =>
                new Profile
                {
                    Name = SelectedProcessModel?.Process.MainModule?.ModuleName ?? string.Empty,
                    Description = SelectedProcessModel?.Process.MainWindowTitle ?? string.Empty,
                    Process = SelectedProcessModel?.Process.MainModule?.ModuleName ?? string.Empty
                }
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
        Processes = new ObservableCollection<ProcessModel>(
            _processMonitorService.RunningProcesses.Items.OrderBy(x => x.Process.ProcessName)
        );
    }
}
