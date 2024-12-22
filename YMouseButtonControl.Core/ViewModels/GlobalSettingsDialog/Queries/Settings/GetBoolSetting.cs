using System.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.Settings.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.Settings;

public static class GetBoolSetting
{
    public sealed class BoolSettingVm(SettingBool setting) : ReactiveObject
    {
        public string Name { get; } = setting.Name;
        public bool Value { get; } = setting.BoolValue;
    }

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public BoolSettingVm Execute(Query q) => new(db.SettingBools.First(x => x.Name == q.Name));
    }
}
