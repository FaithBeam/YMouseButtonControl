using System.Collections.Generic;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes.Models;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes
{
    public interface IListProcessesHandler
    {
        IEnumerable<ProcessModel> Execute();
    }
}
