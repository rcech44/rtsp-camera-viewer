using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Kamery.Core.Services;

namespace Kamery.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    public List<Camera> Source { get; set; } = new List<Camera>();
    public static bool EventLogsChecked { get; set; } = false;
    public static bool EventLogsFound { get; set; } = false;
    public static DateTime LastEventLog { get; set; } = DateTime.Now;

    public MainViewModel()
    {
        LoadCameras();
    }
    private async void LoadCameras()
    {
        Source = CameraDataService.GetAllCameras().ToList();
    }
}
