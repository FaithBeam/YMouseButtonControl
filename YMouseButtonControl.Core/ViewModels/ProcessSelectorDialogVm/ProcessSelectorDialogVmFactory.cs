using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm;

public interface IProcessSelectorDialogVmFactory
{
    ProcessSelectorDialogViewModel Create(string? moduleName = null);
}

public class ProcessSelectorDialogVmFactory(
    IListProcessesHandler listProcessesHandler,
    GetMaxProfileId.Handler getMaxProfileIdHandler
) : IProcessSelectorDialogVmFactory
{
    public ProcessSelectorDialogViewModel Create(string? moduleName = null) =>
        new(listProcessesHandler, getMaxProfileIdHandler, moduleName);
}
