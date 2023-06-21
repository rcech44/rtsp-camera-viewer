using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Kamery.Contracts.ViewModels;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Kamery.Core.Services;
using Newtonsoft.Json;

namespace Kamery.ViewModels;

public partial class DatováMřížkaViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICameraDataService _cameraDataService;

    public ObservableCollection<Camera> Source { get; } = new ObservableCollection<Camera>();

    public DatováMřížkaViewModel(ICameraDataService cameraDataService)
    {
        _cameraDataService = cameraDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _cameraDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public async Task Remove(Camera _camera)
    {
        Source.Remove(_camera);
        await Save();
    }

    public async Task Save()
    {
        await CameraDataService.SaveAllCamerasAsync(Source.ToList());
    }

    public async void Add(Camera _camera)
    {
        Source.Add(_camera);
        await Save();
    }

    public void OnNavigatedFrom()
    {
    }
}
