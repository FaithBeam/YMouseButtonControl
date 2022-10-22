using System.Collections.Generic;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Processes.Interfaces;

public interface IProcessMonitorService
{
    IEnumerable<ProcessModel> GetProcesses();
}