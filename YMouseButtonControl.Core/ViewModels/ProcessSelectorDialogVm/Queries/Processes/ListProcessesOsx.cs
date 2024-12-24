using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes.Models;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes;

public static class ListProcessesOsx
{
    public sealed class Handler : IListProcessesHandler
    {
        public IEnumerable<ProcessModel> Execute() =>
            Process
                .GetProcesses()
                .Select(x => new ProcessModel(x))
                .Where(x => !string.IsNullOrWhiteSpace(x.Process.ProcessName));
    }
}
