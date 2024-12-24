using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.Models;

public class ProfileVmConverter(IProfilesCache profilesCache) : JsonConverter<ProfileVm>
{
    public override void WriteJson(JsonWriter writer, ProfileVm? value, JsonSerializer serializer)
    {
        JObject obj =
            new()
            {
                // Serialize the properties directly.
                ["IsDefault"] = value!.IsDefault,
                ["Checked"] = value.Checked,
                ["Name"] = value.Name,
                ["Description"] = value.Description,
                ["WindowCaption"] = value.WindowCaption,
                ["Process"] = value.Process,
                ["WindowClass"] = value.WindowClass,
                ["ParentClass"] = value.ParentClass,
                ["MatchType"] = value.MatchType,
                ["DisplayPriority"] = value.DisplayPriority,
            };

        if (value.ButtonMappings != null)
        {
            obj["ButtonMappings"] = JToken.FromObject(value.ButtonMappings, serializer);
        }

        obj.WriteTo(writer);
    }

    public override ProfileVm ReadJson(
        JsonReader reader,
        Type objectType,
        ProfileVm? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        JObject obj = JObject.Load(reader);

        // Deserialize the properties directly.
        bool isDefault = obj["IsDefault"]!.ToObject<bool>(serializer);
        bool @checked = obj["Checked"]!.ToObject<bool>(serializer);
        string name = obj["Name"]!.ToObject<string>(serializer)!;
        string description = obj["Description"]?.ToObject<string>(serializer) ?? string.Empty;
        string windowCaption = obj["WindowCaption"]?.ToObject<string>(serializer) ?? "N/A";
        string process = obj["Process"]?.ToObject<string>(serializer) ?? "N/A";
        string windowClass = obj["WindowClass"]?.ToObject<string>(serializer) ?? "N/A";
        string parentClass = obj["ParentClass"]?.ToObject<string>(serializer) ?? "N/A";
        string matchType = obj["MatchType"]?.ToObject<string>(serializer) ?? "N/A";
        int displayPriority = obj["DisplayPriority"]!.ToObject<int>(serializer);

        // Deserialize the button mappings.
        List<BaseButtonMappingVm> buttonMappings = [];
        if (obj["ButtonMappings"] != null)
        {
            buttonMappings = obj["ButtonMappings"]!.ToObject<List<BaseButtonMappingVm>>(
                serializer
            )!;
        }
        var maxBtnMappingIdInCache = profilesCache
            .Profiles.SelectMany(x => x.ButtonMappings)
            .Max(x => x.Id);
        foreach (var bm in buttonMappings)
        {
            bm.Id = ++maxBtnMappingIdInCache;
        }

        var idForProfile = profilesCache.Profiles.Max(x => x.Id) + 1;
        foreach (var bm in buttonMappings)
        {
            bm.ProfileId = idForProfile;
        }

        ProfileVm profileVm =
            new(buttonMappings)
            {
                Id = idForProfile,
                IsDefault = isDefault,
                Checked = @checked,
                Name = name!,
                Description = description,
                WindowCaption = windowCaption,
                Process = process,
                WindowClass = windowClass,
                ParentClass = parentClass,
                MatchType = matchType,
                DisplayPriority = displayPriority,
            };

        return profileVm;
    }
}
