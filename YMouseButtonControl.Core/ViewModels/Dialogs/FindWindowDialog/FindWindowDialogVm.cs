using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

public partial class FindWindowDialogVm : ReactiveObject, IActivatableViewModel
{
    private ObservableAsPropertyHelper<Response?>? _response;

    public FindWindowDialogVm(
        IMouseListener mouseListener,
        IWindowUnderCursorHandler windowUnderCursorHandler,
        ILogger<FindWindowDialogVm> logger
    )
    {
        Activator = new ViewModelActivator();

        this.WhenActivated(d =>
        {
            _response = mouseListener
                .OnMouseDragged.Sample(TimeSpan.FromSeconds(0.5))
                .Where(x => x.Button == YMouseButton.MouseButton1)
                .WhereNotNull()
                .Select(e =>
                    windowUnderCursorHandler.Execute(
                        new Queries.WindowUnderCursor.Models.Query(e.X, e.Y)
                    )
                )
                .ToProperty(this, nameof(Response))
                .DisposeWith(d);
            //mouseListener.OnMouseMovedChanged.Subscribe(x => LogPath(logger, x)).DisposeWith(d);
        });
    }

    public Response? Response => _response?.Value;

    public ViewModelActivator Activator { get; }

    [LoggerMessage(LogLevel.Information, "{Path}")]
    private static partial void LogPath(ILogger logger, NewMouseHookMoveEventArgs path);
}
