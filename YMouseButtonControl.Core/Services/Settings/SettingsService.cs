using System;
using System.Linq;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.Settings;

public interface ISettingsService
{
    IObservable<IChangeSet<BaseSettingVm, int>> Connect();
    bool IsUnsavedChanges();
    BaseSettingVm? GetSetting(string name);
    SettingBoolVm? GetBoolSetting(string name);
    BaseSettingVm? UpdateSetting(int id, BaseSettingVm vm);
    void Save();
    SettingBoolVm? UpdateSetting(SettingBoolVm vm);
}

public class SettingsService : ReactiveObject, ISettingsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SourceCache<BaseSettingVm, int> _settings;

    public SettingsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _settings = new SourceCache<BaseSettingVm, int>(x => x.Id);
        LoadSettingsFromDb();
    }

    public IObservable<IChangeSet<BaseSettingVm, int>> Connect() => _settings.Connect();

    public bool IsUnsavedChanges()
    {
        var dbSettings = _unitOfWork.SettingRepo.Get().ToList();
        return _settings.Items.Count() != dbSettings.Count
            || _settings.Items.Where((p, i) => !p.Equals(dbSettings[i])).Any();
    }

    public BaseSettingVm? GetSetting(string name) =>
        _unitOfWork.SettingRepo.Get().FirstOrDefault(x => x.Name == name);

    public SettingBoolVm? GetBoolSetting(string name) =>
        _unitOfWork.SettingBoolRepo.Get().FirstOrDefault(x => x.Name == name);

    public BaseSettingVm? UpdateSetting(int id, BaseSettingVm vm)
    {
        var dbSetting = _unitOfWork.SettingRepo.GetById(id);
        if (dbSetting is null)
        {
            return null;
        }
        _unitOfWork.SettingRepo.Update(dbSetting.Id, vm);
        return dbSetting;
    }

    public SettingBoolVm? UpdateSetting(SettingBoolVm vm)
    {
        return _unitOfWork.SettingBoolRepo.Update(vm.Id, vm);
    }

    public void Save() => _unitOfWork.Save();

    private void LoadSettingsFromDb()
    {
        var model = _unitOfWork.SettingRepo.Get();
        _settings.AddOrUpdate(model);
    }
}
