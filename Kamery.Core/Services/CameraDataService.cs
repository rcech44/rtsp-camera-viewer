using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Newtonsoft.Json;

namespace Kamery.Core.Services;

// This class holds sample data used by some generated pages to show how they can be used.
// TODO: The following classes have been created to display sample data. Delete these files once your app is using real data.
// 1. Contracts/Services/ISampleDataService.cs
// 2. Services/SampleDataService.cs
// 3. Models/SampleCompany.cs
// 4. Models/SampleOrder.cs
// 5. Models/SampleOrderDetail.cs
public class CameraDataService : ICameraDataService
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

    public async Task<IEnumerable<Camera>> GetGridDataAsync()
    {
        _allCameras = new List<Camera>(AllCameras());

        await Task.CompletedTask;
        return _allCameras;
    }

    public async Task<IEnumerable<Camera>> GetListDetailsDataAsync()
    {
        _allCameras = new List<Camera>(AllCameras());

        await Task.CompletedTask;
        return _allCameras;
    }
}
