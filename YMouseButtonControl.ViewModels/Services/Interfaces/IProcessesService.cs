using System.Collections.Generic;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.ViewModels.Services.Interfaces;

public interface IProcessesService
{
    IEnumerable<ProcessModel> GetProcesses();
}