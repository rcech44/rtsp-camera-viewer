using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KameryPlayer.Models;
using Newtonsoft.Json;

namespace KameryPlayer.Services
{
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
        public CameraPlayerSettings LoadSettings()
        {
            var json = File.ReadAllText("cameraplayer.json");
            return JsonConvert.DeserializeObject<CameraPlayerSettings>(json);
        }
    }
}