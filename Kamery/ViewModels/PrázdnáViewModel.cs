using CommunityToolkit.Mvvm.ComponentModel;
using Kamery.Core.Models;
using Kamery.Core.Services;

namespace Kamery.ViewModels;

public partial class Testování_kamerViewModel : ObservableRecipient
{

    public List<Camera> Source { get; } = new List<Camera>();

    public Testování_kamerViewModel()
    {
        Source = CameraDataService.AllCameras().ToList();
    }
}
