using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Newtonsoft.Json;

namespace Kamery.Core.Services;
public class CameraPlayerSettingsService
{
    public CameraPlayerSettingsService() {}
    public void SaveSettings(CameraPlayerSettings settings)
    {
        var json = JsonConvert.SerializeObject(settings);
        File.WriteAllText("cameraplayer.json", json);
    }
    public CameraPlayerSettings LoadSettings()
    {
        var json = File.ReadAllText("cameraplayer.json");
        return JsonConvert.DeserializeObject<CameraPlayerSettings>(json);
    }
}