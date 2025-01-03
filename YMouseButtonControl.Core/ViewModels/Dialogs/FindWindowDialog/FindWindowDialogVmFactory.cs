﻿using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

public interface IFindWindowDialogVmFactory
{
    FindWindowDialogVm Create();
}

public class FindWindowDialogVmFactory(
    IMouseListener mouseListener,
    IWindowUnderCursorHandler windowUnderCursorHandler,
    ILogger<FindWindowDialogVm> logger
) : IFindWindowDialogVmFactory
{
    public FindWindowDialogVm Create() => new(mouseListener, windowUnderCursorHandler, logger);
}