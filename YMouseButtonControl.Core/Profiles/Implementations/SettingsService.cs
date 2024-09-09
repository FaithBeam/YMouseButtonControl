using System;
using System.Linq;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;

namespace YMouseButtonControl.Core.Profiles.Implementations;

public class SettingsService : ReactiveObject, ISettingsService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly SourceCache<Setting, int> _settings;

    public SettingsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _settings = new SourceCache<Setting, int>(x => x.Id);
        CheckDefaultSettings();
        LoadSettingsFromDb();
    }

    public IObservable<IChangeSet<Setting, int>> Connect() => _settings.Connect();

    public bool IsUnsavedChanges()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Setting>();
        var dbSettings = repository.GetAll().ToList();
        return _settings.Items.Count() != dbSettings.Count
            || _settings.Items.Where((p, i) => !p.Equals(dbSettings[i])).Any();
    }

    public Setting? GetSetting(string name)
    {
        return _settings.Items.FirstOrDefault(x => x.Name == name);
    }

    private void LoadSettingsFromDb()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Setting>();
        var model = repository.GetAll();
        _settings.AddOrUpdate(model);
    }

    private void CheckDefaultSettings()
    {
        using var uow = _unitOfWorkFactory.Create();
        var repo = uow.GetRepository<Setting>();
        var model = repo.GetAll();
        if (model.Any(x => x.Name == "StartMinimized"))
        {
            return;
        }

        var startMinimizedSetting = new Setting { Name = "StartMinimized", Value = "true" };
        repo.Add(startMinimizedSetting);

        uow.SaveChanges();
    }
}
