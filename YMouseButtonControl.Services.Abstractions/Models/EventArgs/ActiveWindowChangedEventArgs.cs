namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class ActiveWindowChangedEventArgs
{
    public ActiveWindowChangedEventArgs(string activeWindow)
    {
        ActiveWindow = activeWindow;
    }

    public string ActiveWindow { get; }
}
