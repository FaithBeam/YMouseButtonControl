using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes;

public static class ListProcessesLinux
{
    public sealed class Handler : IListProcessesHandler
    {
        public IEnumerable<Models.ProcessModel> Execute() =>
            Process
                .GetProcesses()
                .Select(x => new Models.ProcessModel(x))
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Process.MainModule?.ModuleName)
                    && !string.IsNullOrWhiteSpace(x.Process.ProcessName)
                );
    }
}
