using System.Collections.Generic;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes
{
    public interface IListProcessesHandler
    {
        IEnumerable<ProcessModel> Execute();
    }
}
