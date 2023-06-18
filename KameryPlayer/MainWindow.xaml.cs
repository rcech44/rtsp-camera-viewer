using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KameryPlayer.Models;
using KameryPlayer.Services;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace KameryPlayer
{
    public partial class MainWindow : Window
    {
        LibVLC _libVLC;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        CameraPlayerSettings _settings;
        List<Camera> _cameras;
        List<RowDefinition> _rowsDefinitions = new();
        List<ColumnDefinition> _columnsDefinitions = new();
        int _usedCameraIndex = 0;
        bool _isCameraZoomed = false;

        public MainWindow()
        {
            Core.Initialize();

            _settings = new CameraPlayerSettingsService().LoadSettings();
            _cameras = CameraDataService.AllCameras().ToList();
            InitializeComponent();

            if (_settings.Fullscreen)
            {
                //VLCWindow.WindowState = WindowState.Maximized;
                //VLCWindow.WindowStyle = WindowStyle.None;
            }

            for (int i = 0; i < _settings.NumberOfRows; i++)
            {
                var def = new RowDefinition();
                _rowsDefinitions.Add(def);
                VLCGrid.RowDefinitions.Add(def);
            }
            for (int i = 0; i < _settings.NumberOfCols; i++)
            {
                var def = new ColumnDefinition();
                _columnsDefinitions.Add(def);
                VLCGrid.ColumnDefinitions.Add(def);
            }
            for (int i = 0; i < _settings.NumberOfRows; i++)
            {
                for (int j = 0; j < _settings.NumberOfCols; j++)
                {
                    var videoView = new VideoView();
                    videoView.Background = System.Windows.Media.Brushes.Black;
                    videoView.SetValue(Grid.RowProperty, i);
                    videoView.SetValue(Grid.ColumnProperty, j);
                    videoView.Loaded += LoadVLCVideoView;
                    videoView.HorizontalAlignment = HorizontalAlignment.Stretch;
                    videoView.VerticalAlignment = VerticalAlignment.Stretch;
                    //videoView.MouseDown += VideoView_MouseDoubleClick;

                    Canvas b = new Canvas();
                    // if (_cameras.Count != _usedCameraIndex)
                    {
                        videoView.Uid = "_" + _usedCameraIndex.ToString();
                        b.Uid = "_" + _usedCameraIndex++.ToString();
                    }
                    b.HorizontalAlignment = HorizontalAlignment.Stretch;
                    b.VerticalAlignment = VerticalAlignment.Stretch;
                    b.MouseLeftButtonDown += VideoView_MouseDoubleClick;
                    b.KeyDown += HandleKeyPress;
                    b.Background = new BrushConverter().ConvertFromString("#01000000") as SolidColorBrush;
                    videoView.Content = b;
                    

                    VLCGrid.Children.Add(videoView);
                }
            }
            _usedCameraIndex = 0;
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("xd");
        }

        void LoadVLCVideoView(object sender, RoutedEventArgs e)
        {
            Camera c;
            if (_cameras.Count == _usedCameraIndex)
            {
                _libVLC = new LibVLC();
                _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
                ((VideoView)sender).MediaPlayer = _mediaPlayer;
                //c = _cameras[_usedCameraIndex++];
                var address = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
                _mediaPlayer.Play(new Media(_libVLC, new Uri(address)));
                // ((VideoView)sender).Visibility = System.Windows.Visibility.Hidden;
                return;
            }
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
        }

        private void VideoView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("test");
            var list = (System.Collections.IList)VLCGrid.Children;
            if (!_isCameraZoomed)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    VideoView? videoView = (VideoView)list[i];

                    if (videoView != null && ("_" + videoView.Uid) == ("_" + ((Canvas)sender).Uid))
                    {
                        var _curCol = Convert.ToInt32(videoView.Uid.Remove(0,1)) % _columnsDefinitions.Count;
                        var _curRow = Convert.ToInt32(videoView.Uid.Remove(0,1)) / _columnsDefinitions.Count;
                        for (int col = 0; col < _columnsDefinitions.Count; col++)
                        {
                            if (col != _curCol)
                            {
                                _columnsDefinitions[col].MaxWidth = 0;
                                _columnsDefinitions[col].Width = new GridLength(0);
                            }
                        }
                        for (int row = 0; row < _rowsDefinitions.Count; row++)
                        {
                            if (row != _curRow)
                            {
                                _rowsDefinitions[row].MaxHeight = 0;
                                _rowsDefinitions[row].Height = new GridLength(0);
                            }
                        }
                            
                    }
                }
                _isCameraZoomed = true;
            }
            else
            {
                for (int col = 0; col < _columnsDefinitions.Count; col++)
                {
                    _columnsDefinitions[col].MaxWidth = 9999;
                    _columnsDefinitions[col].Width = new GridLength(1, GridUnitType.Star);
                }
                    
                for (int row = 0; row < _rowsDefinitions.Count; row++)
                {
                    _rowsDefinitions[row].MaxHeight = 9999;
                    _rowsDefinitions[row].Height = new GridLength(1, GridUnitType.Star);
                }
                    
                _isCameraZoomed = false;
            }
        }
    }
}
