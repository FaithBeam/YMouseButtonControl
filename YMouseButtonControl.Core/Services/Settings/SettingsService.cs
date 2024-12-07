using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Services.Settings;

public interface ISettingsService
{
    BaseSettingVm? GetSetting(string name);
    int UpdateSetting(BaseSettingVm vm);
}

public class SettingsService(IRepository<Setting, BaseSettingVm> settingRepository)
    : ReactiveObject,
        ISettingsService
{
    public BaseSettingVm? GetSetting(string name) => settingRepository.GetByName(name);

    public int UpdateSetting(BaseSettingVm vm) => settingRepository.Update(vm);
}
