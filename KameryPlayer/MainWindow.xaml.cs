using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Kamery.Core.Models;
using Kamery.Core.Services;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace Kamery.Player;

public partial class MainWindow : Window
{
    private LibVLC? _libVLC;
    private LibVLCSharp.Shared.MediaPlayer? _mediaPlayer;
    private CameraPlayerSettings? _settings;
    private List<Camera>? _cameras;
    private readonly List<RowDefinition> _rowsDefinitions = new();
    private readonly List<ColumnDefinition> _columnsDefinitions = new();
    private int _usedCameraIndex = 0;
    private bool _isCameraZoomed = false;
    private readonly DispatcherTimer _timer = new();

    public MainWindow()
    {
        LibVLCSharp.Shared.Core.Initialize();
        InitializeComponent();
        LoadSettings();
        InitVLCGrid();
        LoadVideoViews();
        if (_settings.Refresh) StartBackgroundTask();
    }
    private void InitVLCGrid()
    {
        // Create grid rows
        for (var i = 0; i < _settings.NumberOfRows; i++)
        {
            var def = new RowDefinition();
            _rowsDefinitions.Add(def);
            VLCGrid.RowDefinitions.Add(def);
        }

        // Create grid columns
        for (var i = 0; i < _settings.NumberOfCols; i++)
        {
            var def = new ColumnDefinition();
            _columnsDefinitions.Add(def);
            VLCGrid.ColumnDefinitions.Add(def);
        }

        // Set fullscreen
        if (_settings.Fullscreen)
        {
            VLCWindow.WindowState = WindowState.Maximized;
            VLCWindow.WindowStyle = WindowStyle.None;
        }
    }
    private void StartBackgroundTask()
    {
        _timer.Interval = TimeSpan.FromMinutes(_settings.RefreshInterval);
        _timer.Tick += RefreshStreams;
        _timer.Start();
    }
    private void RefreshStreams(object sender, EventArgs e)
    {
        VLCGrid.Children.Clear();
        LoadVideoViews();
    }
    private void RefreshStreamsManual()
    {
        VLCGrid.Children.Clear();
        LoadVideoViews();
    }
    private async void LoadSettings()
    {
        _settings = CameraPlayerSettingsService.LoadSettings();
        _cameras = CameraDataService.GetAllCameras().ToList();
        _libVLC = new LibVLC();

        if (_cameras == null || _settings == null)
        {
            ErrorService.ThrowFileError();
        }
    }

    private void LoadVideoViews()
    {
        // Create VLC elements
        for (var row = 0; row < _settings.NumberOfRows; row++)
        {
            for (var col = 0; col < _settings.NumberOfCols; col++)
            {
                var vlcVideoView = new VideoView();
                vlcVideoView.Background = System.Windows.Media.Brushes.Black;
                vlcVideoView.SetValue(Grid.RowProperty, row);
                vlcVideoView.SetValue(Grid.ColumnProperty, col);
                vlcVideoView.Loaded += LoadRTSPStream;
                vlcVideoView.HorizontalAlignment = HorizontalAlignment.Stretch;
                vlcVideoView.VerticalAlignment = VerticalAlignment.Stretch;
                vlcVideoView.Uid = "_" + _usedCameraIndex.ToString();

                Canvas vlcVideoViewCanvas = new Canvas();
                vlcVideoViewCanvas.Uid = "_" + _usedCameraIndex++.ToString();
                vlcVideoViewCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                vlcVideoViewCanvas.VerticalAlignment = VerticalAlignment.Stretch;
                vlcVideoViewCanvas.MouseLeftButtonDown += VideoView_MouseClick;
                vlcVideoViewCanvas.Background = new BrushConverter().ConvertFromString("#01000000") as SolidColorBrush;
                vlcVideoView.Content = vlcVideoViewCanvas;

                VLCGrid.Children.Add(vlcVideoView);
            }
        }
        _usedCameraIndex = 0;
        
    }

    void LoadRTSPStream(object sender, RoutedEventArgs e)
    {
        Camera c;

        // If there is no camera left, fill in with blank spot
        if (_cameras.Count == _usedCameraIndex)
        {
            _libVLC = new LibVLC();
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            ((VideoView)sender).MediaPlayer = _mediaPlayer;
            if (_settings.Volume == true) _mediaPlayer.Volume = 0;
            else _mediaPlayer.Volume = 100;
            //var address = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
            //_mediaPlayer.Play(new Media(_libVLC, new Uri(address)));
            return;
        }

        // Assign mediaplayer to camera in grid
        else
        {
            c = _cameras[_usedCameraIndex++];
            _libVLC = new LibVLC();
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            ((VideoView)sender).MediaPlayer = _mediaPlayer;
            if (_settings.Volume == true) _mediaPlayer.Volume = 0;
            else _mediaPlayer.Volume = 100;
            var address = "rtsp://" + c.LoginName + ":" + c.LoginPassword + "@" + c.Address + "/stream2";
            // var address = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
            _mediaPlayer.Play(new Media(_libVLC, new Uri(address)));
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        window.KeyDown += HandleKeyPress;
    }

    private void HandleKeyPress(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            System.Environment.Exit(0);
        }
        else if (e.Key == Key.R)
        {
            RefreshStreamsManual();
        }
    }

    private void VideoView_MouseClick(object sender, MouseButtonEventArgs e)
    {
        // All VLC windows in grid
        var vlcVideoViews = (System.Collections.IList)VLCGrid.Children;

        // If no camera is zoomed, zoom in selected camera and hide others
        if (!_isCameraZoomed)
        {
            for (var i = 0; i < vlcVideoViews.Count; i++)
            {
                var videoView = (VideoView?)vlcVideoViews[i];

                if (videoView != null)
                if (("_" + videoView.Uid) == ("_" + ((Canvas)sender).Uid))
                {
                    var videoViewColumn = Convert.ToInt32(videoView.Uid.Remove(0,1)) % _columnsDefinitions.Count;
                    var videoViewRow = Convert.ToInt32(videoView.Uid.Remove(0,1)) / _columnsDefinitions.Count;
                    for (var column = 0; column < _columnsDefinitions.Count; column++)
                    {
                        if (column != videoViewColumn)
                        {
                            _columnsDefinitions[column].Width = new GridLength(0);
                        }
                    }
                    for (var row = 0; row < _rowsDefinitions.Count; row++)
                    {
                        if (row != videoViewRow)
                        {
                            _rowsDefinitions[row].Height = new GridLength(0);
                        }
                    }
                        
                }
            }
            _isCameraZoomed = true;
        }

        // If camera is zoomed, reset all cameras to default state
        else
        {
            for (var col = 0; col < _columnsDefinitions.Count; col++)
            {
                _columnsDefinitions[col].Width = new GridLength(1, GridUnitType.Star);
            }
                
            for (var row = 0; row < _rowsDefinitions.Count; row++)
            {
                _rowsDefinitions[row].Height = new GridLength(1, GridUnitType.Star);
            }
                
            _isCameraZoomed = false;
        }
        this.Focus();
    }
}
