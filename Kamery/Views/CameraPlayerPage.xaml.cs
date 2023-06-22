using System.Diagnostics;
using Kamery.Core.Models;
using Kamery.Core.Services;
using Kamery.ViewModels;
using Microsoft.UI.Xaml;
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
        p.StartInfo.FileName = "Kamery.Player.exe";
        p.Start();
        Thread.Sleep(1000);
    }

    public CameraPlayerPage()
    {
        Cameras = CameraDataService.GetAllCameras().ToList();
        ViewModel = App.GetService<CameraPlayerViewModel>();
        InitializeComponent();
        CameraPlayerSettings settings = CameraPlayerSettingsService.LoadSettings();
        FullscreenSwitch.IsOn = settings.Fullscreen;
        VolumeSwitch.IsOn = settings.Volume;
        SliderRow.Value = settings.NumberOfRows;
        SliderColumn.Value = settings.NumberOfCols;
        RefreshSwitch.IsOn = settings.Refresh;
        RefreshIntervalBox.Value = settings.RefreshInterval;
        if (!settings.Refresh) RefreshIntervalBox.IsEnabled = false;
    }

    private void SliderRow_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        SliderRow.Header = "Nastavení řádků v mřížce:   " + e.NewValue;
        if (SliderColumn != null)
        {
            if (SliderRow.Value * SliderColumn.Value < Cameras.Count)
            {
                InfoBarCameraCountWarn.IsOpen = true;
                InfoBarCameraCountWarn.Title = "Varování";
                InfoBarCameraCountWarn.Severity = InfoBarSeverity.Error;
                InfoBarCameraCountWarn.Message = "Počet kamer je vyšší než počet možných kamer v mřížce.";
            }
            else
            {
                InfoBarCameraCountWarn.IsOpen = true;
                InfoBarCameraCountWarn.Title = "Správně";
                InfoBarCameraCountWarn.Severity = InfoBarSeverity.Success;
                InfoBarCameraCountWarn.Message = "Všechny zadané kamery se vlezou do mřížky.";
            }
        }
    }

    private void SliderColumn_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        SliderColumn.Header = "Nastavení sloupců v mřížce:   " + e.NewValue;
        if (SliderRow != null)
        {
            if (SliderRow.Value * SliderColumn.Value < Cameras.Count)
            {
                InfoBarCameraCountWarn.IsOpen = true;
                InfoBarCameraCountWarn.Title = "Varování";
                InfoBarCameraCountWarn.Severity = InfoBarSeverity.Error;
                InfoBarCameraCountWarn.Message = "Počet kamer je vyšší než počet možných kamer v mřížce.";
            }
            else
            {
                InfoBarCameraCountWarn.IsOpen = true;
                InfoBarCameraCountWarn.Title = "Správně";
                InfoBarCameraCountWarn.Severity = InfoBarSeverity.Success;
                InfoBarCameraCountWarn.Message = "Všechny zadané kamery se vlezou do mřížky.";
            }
        }
    }

    private void SaveAndPlay(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var fullScreen = FullscreenSwitch.IsOn;
        var volume = VolumeSwitch.IsOn;
        var rows = (int)SliderRow.Value;
        var cols = (int)SliderColumn.Value;
        var refresh = RefreshSwitch.IsOn;
        var refreshInterval = (int)RefreshIntervalBox.Value;
        var newSettings = new CameraPlayerSettings { Fullscreen = fullScreen, Volume = volume, NumberOfCols = cols, NumberOfRows = rows, Refresh = refresh, RefreshInterval = refreshInterval };
        CameraPlayerSettingsService.SaveSettings(newSettings);
        LaunchPlayer();
    }

    private void RefreshSwitch_Toggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (RefreshSwitch.IsOn) RefreshIntervalBox.IsEnabled = true;
        else RefreshIntervalBox.IsEnabled = false;
    }
}
