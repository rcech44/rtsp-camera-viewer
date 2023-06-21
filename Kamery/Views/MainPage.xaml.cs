using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.WinUI.UI;
using Kamery.Core.Models;
using Kamery.Core.Services;
using Kamery.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;

namespace Kamery.Views;

public sealed partial class MainPage : Page
{
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    public MainViewModel ViewModel
    {
        get;
    }

    private List<Camera> Cameras
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        Cameras = CameraDataService.GetAllCameras().ToList();
        InitializeComponent();
        CameraCountText.Text = "Na této obrazovce je možné spouštět kamery. Momentálně je uloženo " + Cameras.Count.ToString() + " kamer.";
    }

    public void LaunchCamera(Camera c)
    {
        // TODO: Add start location

        var address = $"rtsp://{c.LoginName}:{c.LoginPassword}@{c.Address}/stream2";
        Process p = new Process();
        p.StartInfo.FileName = "C:\\Program Files\\VideoLAN\\VLC\\vlc.exe";
        p.StartInfo.Arguments = $"{address} --qt-minimal-view";
        p.Start();
        Thread.Sleep(1000);
        MainPage.MoveWindow(p.MainWindowHandle, 0, 0, 500, 500, true);
    }

    public void LaunchPlayer()
    {
        Process p = new Process();
        p.StartInfo.FileName = "Kamery.Player.exe";
        p.Start();
        Thread.Sleep(1000);
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (Camera c in Cameras) 
        {
            LaunchCamera(c);
            
        }
    }

    private async void PlayAllCameras(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        LaunchPlayer();
    }

    private void PlayCamera(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        DependencyObject parent = VisualTreeHelper.GetParent((Button)sender);

        while (parent != null && !(parent is GridViewItem))
        {
            parent = VisualTreeHelper.GetParent(parent);
        }

        var cameraId = long.MaxValue;
        if (parent is GridViewItem gridViewItem)
        {
            cameraId = ((Camera)gridViewItem.Content).Id;
        }

        Camera? c = Cameras.Where(x => x.Id == cameraId).FirstOrDefault();
        if (c != null)
        {
            LaunchCamera(c);
        }
    }
}
