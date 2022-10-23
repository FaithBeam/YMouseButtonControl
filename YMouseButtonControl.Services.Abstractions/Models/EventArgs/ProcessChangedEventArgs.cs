namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class ProcessChangedEventArgs : System.EventArgs
{
    public ProcessModel ProcessModel { get; }

    public ProcessChangedEventArgs(ProcessModel processModel)
    {
        ProcessModel = processModel;
    }
}