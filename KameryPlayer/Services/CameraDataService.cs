using System.Collections.Generic;
using System.IO;
using KameryPlayer.Models;
using Newtonsoft.Json;

namespace KameryPlayer.Services
{
    public class CameraDataService
    {
        private List<Camera> _allCameras;

        public CameraDataService()
        {
        }

        public static IEnumerable<Camera> AllCameras()
        {
            var json = File.ReadAllText("cameras.json");
            var objects = JsonConvert.DeserializeObject<Camera[]>(json);

            return objects;
        }
    }
}