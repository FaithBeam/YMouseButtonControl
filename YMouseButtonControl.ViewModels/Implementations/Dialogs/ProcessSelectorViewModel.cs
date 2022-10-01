using System.Windows.Input;
using Avalonia.Collections;
using JetBrains.Annotations;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.ViewModels.Services.Interfaces;
using ReactiveUI;

namespace YMouseButtonControl.ViewModels.Implementations.Dialogs;

public class ProcessSelectorDialogViewModel : DialogBase, IProcessSelectorDialogViewModel
{
    private readonly IProcessesService _processesService;
    [CanBeNull] private ProcessModel _processModel;

    public ICommand RefreshButtonCommand { get; }

    public ProcessSelectorDialogViewModel(IProcessesService processesService)
    {
        _processesService = processesService;
        RefreshButtonCommand = ReactiveCommand.Create(OnRefreshButtonClicked);
        Processes = new(_processesService.GetProcesses());
    }

    public AvaloniaList<ProcessModel> Processes { get; private set; }

    [CanBeNull]
    public ProcessModel SelectedProcessModel
    {
        get => _processModel;
        set
        {
            _processModel = value;
            this.RaisePropertyChanged(nameof(Application));
            this.RaisePropertyChanged(nameof(Description));
        }
    }

    public string Application
    {
        get => SelectedProcessModel?.ProcessName;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.ProcessName = value;
            this.RaisePropertyChanged(nameof(Processes));
        }
    }

    public string Description
    {
        get => SelectedProcessModel?.WindowTitle;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.WindowTitle = value;
            this.RaisePropertyChanged(nameof(Processes));
        }
    }

    private void OnRefreshButtonClicked()
    {
        Processes = new AvaloniaList<ProcessModel>(_processesService.GetProcesses());
        this.RaisePropertyChanged(nameof(Processes));
    }
}