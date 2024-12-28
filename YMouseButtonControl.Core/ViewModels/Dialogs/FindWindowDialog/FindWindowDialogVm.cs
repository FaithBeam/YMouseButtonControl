using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

public partial class FindWindowDialogVm : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<Response?> _response;

    public FindWindowDialogVm(
        IMouseListener mouseListener,
        IWindowUnderCursorHandler windowUnderCursorHandler
    )
    {
        _response = mouseListener
            .OnMouseMovedChanged.Select(e =>
                windowUnderCursorHandler.Execute(
                    new Queries.WindowUnderCursor.Models.Query(e.X, e.Y)
                )
            )
            .ToProperty(this, x => x.Response);
    }

    public Response? Response => _response.Value;
}
