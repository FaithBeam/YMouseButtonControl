using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;
using static YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartsMinimized.StartsMinimized;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartsMinimized;

public interface IStartsMinimized
{
    StartsMinimizedResponse GetStartsMinimized();
}

public class StartsMinimized(YMouseButtonControlDbContext db) : IStartsMinimized
{
    public class StartsMinimizedResponse : ReactiveObject
    {
        private bool _value;

        public StartsMinimizedResponse(SettingBool settingBool)
        {
            Value = settingBool.BoolValue;
        }

        public bool Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public StartsMinimizedResponse GetStartsMinimized() =>
        new(db.SettingBools.AsNoTracking().First(x => x.Name == "StartMinimized"));
}
