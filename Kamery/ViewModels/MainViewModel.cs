using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Kamery.Core.Contracts.Services;
using Kamery.Core.Models;
using Kamery.Core.Services;

namespace Kamery.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    public List<Camera> Source { get; } = new List<Camera>();

    public MainViewModel()
    {
        Source = CameraDataService.AllCameras().ToList();
    }

    public void Button_Clicak()
    {
    
    }
}
