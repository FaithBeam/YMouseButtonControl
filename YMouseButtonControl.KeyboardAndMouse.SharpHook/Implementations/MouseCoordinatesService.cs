using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class MouseCoordinatesService : IMouseCoordinatesService
{
    private readonly IMouseListener _mouseListener;

    public MouseCoordinatesService(IMouseListener mouseListener)
    {
        _mouseListener = mouseListener;
    }

    /// <summary>
    /// Populate the mapping's start and end X and Y coordinates with the current mouse position if they aren't already set
    /// </summary>
    /// <param name="mapping"></param>
    public void HandleMouseCoordinates(ISequencedMapping mapping)
    {
        
    }
}