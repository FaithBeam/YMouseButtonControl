using System;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Processes.Interfaces;

public interface ICurrentWindowService
{
    string ForegroundWindow { get; }
    void Dispose();
    void Run();
}