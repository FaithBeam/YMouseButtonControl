using System;
using System.Threading.Tasks;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DataAccess.Models;
using Profile = YMouseButtonControl.DataAccess.Models.Profile;

namespace YMouseButtonControl.Core.Repositories;

public interface IUnitOfWork
{
    IGenericRepository<Profile, ProfileVm> ProfileRepo { get; init; }
    IGenericRepository<ButtonMapping, BaseButtonMappingVm> ButtonMappingRepo { get; init; }
    IGenericRepository<Setting, BaseSettingVm> SettingRepo { get; init; }
    IGenericRepository<SettingBool, SettingBoolVm> SettingBoolRepo { get; init; }
    IGenericRepository<SettingString, SettingStringVm> SettingStringRepo { get; init; }
    void Save();
    Task SaveAsync();
    void Dispose();
}

public class UnitOfWork(
    YMouseButtonControlDbContext context,
    IGenericRepository<Profile, ProfileVm> profileRepo,
    IGenericRepository<ButtonMapping, BaseButtonMappingVm> buttonMappingRepo,
    IGenericRepository<Setting, BaseSettingVm> settingRepo,
    IGenericRepository<SettingBool, SettingBoolVm> settingBoolRepo,
    IGenericRepository<SettingString, SettingStringVm> settingStringRepo
) : IUnitOfWork
{
    private readonly YMouseButtonControlDbContext _context = context;
    public IGenericRepository<Profile, ProfileVm> ProfileRepo { get; init; } = profileRepo;
    public IGenericRepository<ButtonMapping, BaseButtonMappingVm> ButtonMappingRepo { get; init; } =
        buttonMappingRepo;
    public IGenericRepository<Setting, BaseSettingVm> SettingRepo { get; init; } = settingRepo;
    public IGenericRepository<SettingBool, SettingBoolVm> SettingBoolRepo { get; init; } =
        settingBoolRepo;
    public IGenericRepository<SettingString, SettingStringVm> SettingStringRepo { get; init; } =
        settingStringRepo;

    public void Save() => _context.SaveChanges();

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
