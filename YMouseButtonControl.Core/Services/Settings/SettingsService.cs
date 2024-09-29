using System.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.Settings;

public interface ISettingsService
{
    BaseSettingVm? GetSetting(string name);
    BaseSettingVm? UpdateSetting(BaseSettingVm vm);
    void Save();
}

public class SettingsService(IUnitOfWork unitOfWork) : ReactiveObject, ISettingsService
{
    public BaseSettingVm? GetSetting(string name) =>
        unitOfWork.SettingRepo.Get(x => x.Name == name).FirstOrDefault();

    public BaseSettingVm? UpdateSetting(BaseSettingVm vm) =>
        unitOfWork.SettingRepo.Update(vm.Id, vm);

    public void Save() => unitOfWork.Save();
}
