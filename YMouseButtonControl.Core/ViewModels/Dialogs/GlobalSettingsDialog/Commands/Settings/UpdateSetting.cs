using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.Settings;

public static class UpdateSetting<T>
{
    public sealed record Command(string Name, T Value);

    public sealed class Handler(YMouseButtonControlDbContext db)
    {
        public async Task ExecuteAsync(Command c)
        {
            switch (c.Value)
            {
                case string:
                    var entStr = await db.SettingStrings.FirstAsync(x => x.Name == c.Name);
                    entStr.StringValue = c.Value as string;
                    break;
                case int:
                    var entInt = await db.SettingInts.FirstAsync(x => x.Name == c.Name);
                    entInt.IntValue = Convert.ToInt32(c.Value);
                    break;
                case bool:
                    var entBool = await db.SettingBools.FirstAsync(x => x.Name == c.Name);
                    entBool.BoolValue = Convert.ToBoolean(c.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(c.Value));
            }
            await db.SaveChangesAsync();
        }
    }
}
