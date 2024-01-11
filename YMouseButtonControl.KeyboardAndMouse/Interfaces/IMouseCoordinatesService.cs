using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IMouseCoordinatesService
{
    /// <summary>
    /// Populate the mapping's start and end X and Y coordinates with the current mouse position if they aren't already set
    /// </summary>
    /// <param name="mapping"></param>
    void HandleMouseCoordinates(ISequencedMapping mapping);
}