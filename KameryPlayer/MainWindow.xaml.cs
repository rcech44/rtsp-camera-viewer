using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KameryPlayer.Models;
using KameryPlayer.Services;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace KameryPlayer
{
    public partial class MainWindow : Window
    {
        private LibVLC _libVLC;
        private LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        private readonly CameraPlayerSettings? _settings;
        private readonly List<Camera>? _cameras;
        private List<RowDefinition> _rowsDefinitions = new();
        private List<ColumnDefinition> _columnsDefinitions = new();
        private int _usedCameraIndex = 0;
        private bool _isCameraZoomed = false;

        public MainWindow()
        {
            _settings = new CameraPlayerSettingsService().LoadSettings();
            _cameras = CameraDataService.AllCameras().ToList();

            if ( _cameras == null || _settings == null) 
            {
                ErrorService.ThrowFileError();
            }

            LibVLCSharp.Shared.Core.Initialize();
            InitializeComponent();

            if (_settings.Fullscreen)
            {
                VLCWindow.WindowState = WindowState.Maximized;
                VLCWindow.WindowStyle = WindowStyle.None;
            }

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

            // Create VLC elements
            for (var row = 0; row < _settings.NumberOfRows; row++)
            {
                for (var col = 0; col < _settings.NumberOfCols; col++)
                {
                    var vlcVideoView = new VideoView();
                    vlcVideoView.Background = System.Windows.Media.Brushes.Black;
                    vlcVideoView.SetValue(Grid.RowProperty, row);
                    vlcVideoView.SetValue(Grid.ColumnProperty, col);
                    vlcVideoView.Loaded += LoadVLCVideoView;
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
        void LoadVLCVideoView(object sender, RoutedEventArgs e)
        {
            Camera c;

            // If there is no camera left, fill in with blank spot
            if (_cameras.Count == _usedCameraIndex)
            {
                _libVLC = new LibVLC();
                _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
                ((VideoView)sender).MediaPlayer = _mediaPlayer;
                //c = _cameras[_usedCameraIndex++];
                var address = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
                if (_settings.Volume = true) _mediaPlayer.Volume = 0;
                else _mediaPlayer.Volume = 100;
                _mediaPlayer.Play(new Media(_libVLC, new Uri(address)));
                // ((VideoView)sender).Visibility = System.Windows.Visibility.Hidden;
                return;
            }

            // Assign mediaplayer to camera in grid
            else
            {
                _libVLC = new LibVLC();
                _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
                ((VideoView)sender).MediaPlayer = _mediaPlayer;
                c = _cameras[_usedCameraIndex++];
                //var address = "rtsp://" + c.LoginName + ":" + c.LoginPassword + "@" + c.Address + "/stream2";
                var address = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
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
            if (e.Key == Key.Space)
            {
                MessageBox.Show("test");
            }
        }

        private void VideoView_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var vlcVideoViews = (System.Collections.IList)VLCGrid.Children;

            // If no camera is zoomed, zoom in selected camera and hide others
            if (!_isCameraZoomed)
            {
                for (var i = 0; i < vlcVideoViews.Count; i++)
                {
                    VideoView? videoView = (VideoView?)vlcVideoViews[i];

                    if (videoView != null && ("_" + videoView.Uid) == ("_" + ((Canvas)sender).Uid))
                    {
                        var curCol = Convert.ToInt32(videoView.Uid.Remove(0,1)) % _columnsDefinitions.Count;
                        var curRow = Convert.ToInt32(videoView.Uid.Remove(0,1)) / _columnsDefinitions.Count;
                        for (var col = 0; col < _columnsDefinitions.Count; col++)
                        {
                            if (col != curCol)
                            {
                                _columnsDefinitions[col].MaxWidth = 0;
                                _columnsDefinitions[col].Width = new GridLength(0);
                            }
                        }
                        for (var row = 0; row < _rowsDefinitions.Count; row++)
                        {
                            if (row != curRow)
                            {
                                _rowsDefinitions[row].MaxHeight = 0;
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
                    _columnsDefinitions[col].MaxWidth = 9999;
                    _columnsDefinitions[col].Width = new GridLength(1, GridUnitType.Star);
                }
                    
                for (var row = 0; row < _rowsDefinitions.Count; row++)
                {
                    _rowsDefinitions[row].MaxHeight = 9999;
                    _rowsDefinitions[row].Height = new GridLength(1, GridUnitType.Star);
                }
                    
                _isCameraZoomed = false;
            }
            this.Focus();
        }
    }
}
