using System.Collections.Generic;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Core.Processes;

public interface IProcessMonitorService
{
    IEnumerable<ProcessModel> GetProcesses();
}
