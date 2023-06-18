using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using KameryPlayer.Models;
using Newtonsoft.Json;

namespace KameryPlayer.Services;

public class CameraDataService
{

    public CameraDataService()
    {

    }

    public static IEnumerable<Camera>? AllCameras()
    {
        try
        {
            var json = File.ReadAllText("cameras.json");
            var objects = JsonConvert.DeserializeObject<Camera[]>(json);
            return objects;
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
            return null;
        }
    }
}