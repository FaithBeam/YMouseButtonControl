using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Collections;
using JetBrains.Annotations;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations.Dialogs;

public class ProcessSelectorDialogViewModel : DialogBase, IProcessSelectorDialogViewModel
{
    private readonly IProcessMonitorService _processMonitorService;
    [CanBeNull] private ProcessModel _processModel;

    public ICommand RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, Profile> OkCommand { get; }

    public ProcessSelectorDialogViewModel(IProcessMonitorService processMonitorService)
    {
        _processMonitorService = processMonitorService;
        RefreshButtonCommand = ReactiveCommand.Create(OnRefreshButtonClicked);
        OkCommand = ReactiveCommand.Create(() => new Profile
            { Name = ProcessName, Description = WindowTitle, Process = ProcessName });
        Processes = new AvaloniaList<ProcessModel>(_processMonitorService.GetProcesses().OrderBy(x => x.ProcessName));
    }

    public AvaloniaList<ProcessModel> Processes { get; private set; }

    [CanBeNull]
    public ProcessModel SelectedProcessModel
    {
        get => _processModel;
        set
        {
            _processModel = value;
            this.RaisePropertyChanged(nameof(ProcessName));
            this.RaisePropertyChanged(nameof(WindowTitle));
            this.RaisePropertyChanged(nameof(BitmapPath));
        }
    }

    public string ProcessName
    {
        get => SelectedProcessModel?.ProcessName;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.ProcessName = value;
        }
    }

    public string WindowTitle
    {
        get => SelectedProcessModel?.WindowTitle;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.WindowTitle = value;
        }
    }
    
    [CanBeNull]
    public string BitmapPath
    {
        get => SelectedProcessModel?.BitmapPath;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.BitmapPath = value;
        }
    }

    private void OnRefreshButtonClicked()
    {
        Processes = new AvaloniaList<ProcessModel>(_processMonitorService.GetProcesses().OrderBy(x => x.ProcessName));
        this.RaisePropertyChanged(nameof(Processes));
    }
}