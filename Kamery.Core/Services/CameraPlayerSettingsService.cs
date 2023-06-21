using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Helpers;
using Kamery.Core.Models;
using Newtonsoft.Json;

namespace Kamery.Core.Services;
public class CameraPlayerSettingsService
{
    public CameraPlayerSettingsService() {}
    public static async Task<CameraPlayerSettings?> LoadSettingsAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync("cameraplayer.json");
            var objects = await Json.ToObjectAsync<CameraPlayerSettings>(json);
            return objects;
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
            return null;
        }
    }
    public static CameraPlayerSettings? LoadSettings()
    {
        try
        {
            var json = File.ReadAllText("cameraplayer.json");
            var objects = JsonConvert.DeserializeObject<CameraPlayerSettings>(json);
            return objects;
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
            return null;
        }
    }
    public static void SaveSettings(CameraPlayerSettings settings)
    {
        try
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText("cameraplayer.json", json);
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
        }
    }
}