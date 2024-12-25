using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Profiles;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Themes;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;

public interface IProcessSelectorDialogVmFactory
{
    ProcessSelectorDialogViewModel Create(string? moduleName = null);
}

public class ProcessSelectorDialogVmFactory(
    IListProcessesHandler listProcessesHandler,
    GetMaxProfileId.Handler getMaxProfileIdHandler,
    GetThemeVariant.Handler getThemeVariantHandler
) : IProcessSelectorDialogVmFactory
{
    public ProcessSelectorDialogViewModel Create(string? moduleName = null) =>
        new(listProcessesHandler, getMaxProfileIdHandler, getThemeVariantHandler, moduleName);
}
