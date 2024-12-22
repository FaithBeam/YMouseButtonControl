using System.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.Settings.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.Settings;

public static class GetIntSetting
{
    public sealed class IntSettingVm(SettingInt setting) : ReactiveObject
    {
        private int _value = setting.IntValue;
        public string Name { get; } = setting.Name;
        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public IntSettingVm Execute(Query q) => new(db.SettingInts.First(x => x.Name == q.Name));
    }
}
