using YMouseButtonControl.Services.Environment.Interfaces;

namespace YMouseButtonControl.Services.Environment.Implementations;

public class EnvironmentService : IEnvironmentService
{
    public string NewLine => System.Environment.NewLine;

    public bool Is64BitProcess => System.Environment.Is64BitProcess;

    public string GetEnvironmentVariable(string variableName) =>
        System.Environment.GetEnvironmentVariable(variableName);
}
