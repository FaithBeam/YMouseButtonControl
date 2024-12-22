using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.Themes;

public class ListThemes
{
    public sealed class ThemeVm(Domain.Models.Theme theme)
    {
        public int Id { get; } = theme.Id;
        public string Name { get; } = theme.Name;
    }

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public List<ThemeVm> Execute() => [.. db.Themes.AsNoTracking().Select(x => new ThemeVm(x))];
    }
}
