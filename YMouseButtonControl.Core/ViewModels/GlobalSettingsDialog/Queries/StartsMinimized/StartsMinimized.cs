using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartsMinimized;

public static class StartsMinimized
{
    public sealed class StartsMinimizedVm : ReactiveObject
    {
        private bool _value;

        public StartsMinimizedVm(SettingBool settingBool)
        {
            Value = settingBool.BoolValue;
        }

        public bool Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public StartsMinimizedVm Execute() =>
            new(db.SettingBools.AsNoTracking().First(x => x.Name == "StartMinimized"));
    }
}
