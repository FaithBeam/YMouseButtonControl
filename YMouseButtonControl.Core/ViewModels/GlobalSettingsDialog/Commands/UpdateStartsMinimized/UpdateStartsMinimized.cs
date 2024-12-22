using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.UpdateStartsMinimized;

public static class UpdateStartsMinimized
{
    public sealed record Command(bool Value);

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public async Task ExecuteAsync(Command c)
        {
            var ent = await db.SettingBools.FirstAsync(x => x.Name == "StartMinimized");
            ent.BoolValue = c.Value;
            await db.SaveChangesAsync();
        }
    }
}
