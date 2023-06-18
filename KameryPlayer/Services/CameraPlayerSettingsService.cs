using System;
using System.IO;
using System.Windows;
using KameryPlayer.Models;
using Newtonsoft.Json;

namespace KameryPlayer.Services;

public class CameraPlayerSettingsService
{
    public CameraPlayerSettingsService()
    {
    }
    public void SaveSettings(CameraPlayerSettings settings)
    {
        var json = JsonConvert.SerializeObject(settings);
        File.WriteAllText("cameraplayer.json", json);
    }
    public CameraPlayerSettings? LoadSettings()
    {
        try
        {
            var json = File.ReadAllText("cameraplayer.json");
            return JsonConvert.DeserializeObject<CameraPlayerSettings>(json);
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
            return null;
        }
    }
}