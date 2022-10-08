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
    private readonly IProcessesService _processesService;
    [CanBeNull] private ProcessModel _processModel;

    public ICommand RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, Profile> OkCommand { get; }

    public ProcessSelectorDialogViewModel(IProcessesService processesService)
    {
        _processesService = processesService;
        RefreshButtonCommand = ReactiveCommand.Create(OnRefreshButtonClicked);
        OkCommand = ReactiveCommand.Create(() => new Profile()
            { Name = Description, Description = Description, Process = Application });
        Processes = new AvaloniaList<ProcessModel>(_processesService.GetProcesses());
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
        }
    }

    public string Description
    {
        get => SelectedProcessModel?.WindowTitle;
        set
        {
            if (SelectedProcessModel is null) return;
            SelectedProcessModel.WindowTitle = value;
        }
    }

    private void OnRefreshButtonClicked()
    {
        Processes = new AvaloniaList<ProcessModel>(_processesService.GetProcesses());
        this.RaisePropertyChanged(nameof(Processes));
    }
}