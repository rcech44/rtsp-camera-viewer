using System.Diagnostics;
using Kamery.Core.Models;
using Kamery.Core.Services;
using Kamery.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Kamery.Views;

public sealed partial class CameraPlayerPage : Page
{
    private List<Camera> Cameras
    {
        get;set;
    }
    public CameraPlayerViewModel ViewModel
    {
        get;
    }
    public void LaunchPlayer()
    {
        Process p = new Process();
        p.StartInfo.FileName = "KameryPlayer.exe";
        p.Start();
        Thread.Sleep(1000);
    }

    public CameraPlayerPage()
    {
        ViewModel = App.GetService<CameraPlayerViewModel>();
        Cameras = CameraDataService.AllCameras().ToList();
        InitializeComponent();
        CameraPlayerSettings settings = new CameraPlayerSettingsService().LoadSettings();
        FullscreenSwitch.IsOn = settings.Fullscreen;
        VolumeSwitch.IsOn = settings.Volume;
        SliderRow.Value = settings.NumberOfRows;
        SliderColumn.Value = settings.NumberOfCols;
    }

    private void SliderRow_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        SliderRow.Header = "Nastavení řádků v mřížce:   " + e.NewValue;
        if (SliderColumn != null)
        if (SliderRow.Value * SliderColumn.Value < Cameras.Count )
        {
            InfoBarCameraCountWarn.IsOpen = true;
        }
        else InfoBarCameraCountWarn.IsOpen = false;
    }

    private void SliderColumn_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        SliderColumn.Header = "Nastavení sloupců v mřížce:   " + e.NewValue;
        if (SliderRow != null)
        if (SliderRow.Value * SliderColumn.Value < Cameras.Count)
        {
            InfoBarCameraCountWarn.IsOpen = true;
        }
        else InfoBarCameraCountWarn.IsOpen = false;
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var fullscreen = (bool)FullscreenSwitch.IsOn;
        var volume = (bool)VolumeSwitch.IsOn;
        var rows = (int)SliderRow.Value;
        var cols = (int)SliderColumn.Value;
        var settings = new CameraPlayerSettings { Fullscreen = fullscreen, Volume = volume, NumberOfCols = cols, NumberOfRows = rows };
        new CameraPlayerSettingsService().SaveSettings(settings);
        LaunchPlayer();
    }
}
