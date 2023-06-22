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
        //CheckEventLogsHelper();
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
        //MainPage.MoveWindow(p.MainWindowHandle, 0, 0, 500, 500, true);
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

    private async Task CheckEventLogs()
    {
        try
        {
            var logName = "System"; // Název logu, ve kterém se nachází události
            var source = "Microsoft-Windows-Kernel-Power"; // Název zdroje události
            var count = 0;
            var latestDate = DateTime.Now.AddYears(-1);

            EventLog eventLog = new EventLog(logName);

            foreach (EventLogEntry entry in eventLog.Entries)
            {
                if (entry.Source.Equals(source, StringComparison.OrdinalIgnoreCase) && entry.EventID == 41 && entry.CategoryNumber == 63)
                {
                    if (entry.TimeWritten.CompareTo(DateTime.Now.AddDays(-1)) > 0)
                    {
                        count++;
                        if (entry.TimeWritten > latestDate) latestDate = entry.TimeWritten;
                    }
                }
            }

            eventLog.Close();

            if (count > 0)
            {
                MainViewModel.EventLogsFound = true;
                MainViewModel.LastEventLog = latestDate;
                DispatcherQueue.TryEnqueue(async () =>
                await new ContentDialog
                {
                    Title = "Upozornění na výpadek napájení",
                    Content = "Byl detekován výpadek napájení v počítači za posledních 24 hodin. Je možné, že došlo k výpadku proudu, tudíž se mohly resetovat IP adresy kamer. Zkontrolujte IP adresy kamer. Poslední výpadek proběhl v " + latestDate.ToString(),
                    XamlRoot = this.XamlRoot,
                    CloseButtonText = "Ok, chápu"
                }.ShowAsync()
                );

            }
        }
        catch (Exception ex)
        {
            MainViewModel.EventLogsFound = false;
        }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (!MainViewModel.EventLogsChecked)
        {
            await CheckEventLogs();
            MainViewModel.EventLogsChecked = true;
        }
        if (MainViewModel.EventLogsFound)
        {
            InfoBarElectricityError.IsOpen = true;
            InfoBarElectricityError.Message = "Byl detekován výpadek proudu v " + MainViewModel.LastEventLog.ToString();
        }
    }
}
