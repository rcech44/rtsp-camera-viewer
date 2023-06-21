using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Kamery.Core.Helpers;
using Newtonsoft.Json;
namespace Kamery.Core.Services;

public class CameraDataService : ICameraDataService
{
    private List<Camera> _allCameras;

    public CameraDataService()
    {
    }

    public static async Task<IEnumerable<Camera>> GetAllCamerasAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync("cameras.json");
            var objects = await Json.ToObjectAsync<Camera[]>(json);
            //var objects = JsonConvert.DeserializeObject<Camera[]>(json);
            return objects;
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
            return null;
        }
    }
    public static IEnumerable<Camera> GetAllCameras()
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
    public static void SaveAllCameras(List<Camera> cameras)
    {
        try
        {
            var json = JsonConvert.SerializeObject(cameras);
            File.WriteAllText("cameras.json", json);
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
        }
    }
    public static async Task SaveAllCamerasAsync(List<Camera> cameras)
    {
        try
        {
            var json = await Json.StringifyAsync(cameras);
            await File.WriteAllTextAsync("cameras.json", json);
        }
        catch (Exception)
        {
            ErrorService.ThrowFileError();
        }
    }

    public async Task<IEnumerable<Camera>> GetGridDataAsync()
    {
        _allCameras = new List<Camera>(await GetAllCamerasAsync());

        await Task.CompletedTask;
        return _allCameras;
    }

    public async Task<IEnumerable<Camera>> GetListDetailsDataAsync()
    {
        _allCameras = new List<Camera>(await GetAllCamerasAsync());

        await Task.CompletedTask;
        return _allCameras;
    }
}
