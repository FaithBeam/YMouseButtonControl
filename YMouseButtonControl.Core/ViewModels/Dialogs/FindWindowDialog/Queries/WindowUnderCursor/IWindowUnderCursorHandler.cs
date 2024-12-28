using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor
{
    public interface IWindowUnderCursorHandler
    {
        Response? Execute(Query q);
    }
}
