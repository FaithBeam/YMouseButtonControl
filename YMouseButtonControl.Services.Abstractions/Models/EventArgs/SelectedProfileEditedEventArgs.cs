using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class SelectedProfileEditedEventArgs : System.EventArgs
{
    public IButtonMapping Mapping { get; }
    public MouseButton Button { get; }

    public SelectedProfileEditedEventArgs(IButtonMapping mapping, MouseButton button)
    {
        Mapping = mapping;
        Button = button;
    }
}