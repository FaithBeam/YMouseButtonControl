using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.MouseCombo.Queries.Theme;

public static class GetThemeBackground
{
    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public IBrush Execute()
        {
            var themeId = db.SettingInts.First(x => x.Name == "Theme").IntValue;
            var themeBackground =
                db.Themes.Find(themeId)?.Background
                ?? throw new Exception($"Error retrieving theme background for id {themeId}");
            return GetBackground(themeBackground);
        }

        private static IBrush GetBackground(string background)
        {
            // Background is of the form #aarrggbb
            if (background.StartsWith('#'))
            {
                return Brush.Parse(background);
            }

            // Background is an avalonia resource like SystemAltHighColor
            if (
                Application.Current!.TryGetResource(
                    background,
                    Application.Current.ActualThemeVariant,
                    out var backgroundBrush
                )
            )
            {
                if (backgroundBrush is null)
                {
                    throw new Exception("Error retrieving background brush");
                }
                var bbStr = backgroundBrush.ToString();
                if (string.IsNullOrWhiteSpace(bbStr))
                {
                    throw new Exception("Error retrieving background brush");
                }
                var brush = Brush.Parse(bbStr);
                return brush;
            }

            // Background may be a color like White, Black, etc.
            return Brush.Parse(background);
        }
    }
}
