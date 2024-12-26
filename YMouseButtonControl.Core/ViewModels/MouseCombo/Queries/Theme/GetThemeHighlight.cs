using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.MouseCombo.Queries.Theme;

public static class GetThemeHighlight
{
    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        private static IBrush? Highlight;

        public IBrush Execute()
        {
            if (Highlight is not null)
            {
                return Highlight;
            }
            var themeId = db.SettingInts.First(x => x.Name == "Theme").IntValue;
            var themeBackground =
                db.Themes.Find(themeId)?.Highlight
                ?? throw new Exception($"Error retrieving theme highlight for id {themeId}");
            Highlight = GetHighlight(themeBackground);
            return Highlight;
        }

        private static IBrush GetHighlight(string highlight)
        {
            // Highlight is of the form #aarrggbb
            if (highlight.StartsWith('#'))
            {
                return Brush.Parse(highlight);
            }

            // Highlight is an avalonia resource like SystemAltHighColor
            if (
                Application.Current!.TryGetResource(
                    highlight,
                    Application.Current.ActualThemeVariant,
                    out var highlightBrush
                )
            )
            {
                if (highlightBrush is null)
                {
                    throw new Exception("Error retrieving highlight brush");
                }
                var bbStr = highlightBrush.ToString();
                if (string.IsNullOrWhiteSpace(bbStr))
                {
                    throw new Exception("Error retrieving highlight brush");
                }
                var brush = Brush.Parse(bbStr);
                return brush;
            }

            // Highlight may be a color like White, Black, etc.
            return Brush.Parse(highlight);
        }
    }
}
