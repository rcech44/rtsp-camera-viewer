using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Kamery.Core.Services;

namespace Kamery.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    public List<Camera> Source { get; set; } = new List<Camera>();

    public MainViewModel()
    {
        LoadCameras();
    }
    private async void LoadCameras()
    {
        Source = (await CameraDataService.GetAllCamerasAsync()).ToList();
    }
}
