using System.Linq;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Queries.Settings;

internal static class GetBoolSetting
{
    internal sealed record Query(string Name);

    internal sealed class Handler(YMouseButtonControlDbContext db)
    {
        internal bool Execute(Query q) => db.SettingBools.First(x => x.Name == q.Name).BoolValue;
    }
}
