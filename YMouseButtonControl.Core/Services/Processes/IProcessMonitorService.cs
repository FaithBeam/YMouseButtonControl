using System.Collections.Generic;

namespace YMouseButtonControl.Core.Services.Processes;

public interface IProcessMonitorService
{
    IEnumerable<ProcessModel> GetProcesses();
}
