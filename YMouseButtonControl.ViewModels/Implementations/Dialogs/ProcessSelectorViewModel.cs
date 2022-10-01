using Avalonia.Collections;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations.Dialogs;

public class ProcessSelectorDialogViewModel : IProcessSelectorDialogViewModel
{
    private readonly IProcessesService _processesService;

    public ProcessSelectorDialogViewModel(IProcessesService processesService)
    {
        _processesService = processesService;
    }

    public AvaloniaList<ProcessModel> Processes => new(_processesService.GetProcesses());
}