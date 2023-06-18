using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Kamery.Contracts.ViewModels;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
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

    public void Remove(Camera _camera)
    {
        Source.Remove(_camera);
        Save();
    }

    public void Save()
    {
        var list = Source.ToList();
        var json = JsonConvert.SerializeObject(list);

        File.WriteAllText("cameras.json", json);
    }

    public void Add(Camera _camera)
    {
        Source.Add(_camera);
        Save();
    }

    public void OnNavigatedFrom()
    {
    }
}
