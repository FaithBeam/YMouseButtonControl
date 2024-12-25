using System;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Theme;

public static class GetThemeVariant
{
    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public ThemeVariant Execute() =>
            db.SettingInts.First(x => x.Name == "Theme").IntValue switch
            {
                1 => Application.Current!.ActualThemeVariant == ThemeVariant.Light
                    ? ThemeVariant.Light
                    : ThemeVariant.Dark,
                2 => ThemeVariant.Light,
                3 => ThemeVariant.Dark,
                _ => throw new ArgumentOutOfRangeException($"Invalid theme id"),
            };
    }
}
