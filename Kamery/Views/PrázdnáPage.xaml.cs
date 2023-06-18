using Kamery.Core.Models;
using Kamery.ViewModels;
using Kamery.Helpers;
//using LibVLCSharp.Platforms.Windows;
//using LibVLCSharp.Shared;


using Microsoft.UI.Xaml.Controls;

namespace Kamery.Views;

public sealed partial class Testování_kamerPage : Page
{
    //LibVLC _libvlc;
    //MediaPlayer mp;

    public Testování_kamerViewModel ViewModel
    {
        get;
    }

    public Testování_kamerPage()
    {
        ViewModel = App.GetService<Testování_kamerViewModel>();
        InitializeComponent();
        //_libvlc = new LibVLC();
        VideoView_Initialized();

    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var c = (Camera)ComboBox.SelectedItem;
        var address = $"rtsp://{c.LoginName}:{c.LoginPassword}@{c.Address}/stream2";
        var encoder = new Base64Converter();
        var encoded_address = encoder.Encode(address);
        MyWebView2.Source = new Uri("https://streamedian.com/embed?s=" + encoded_address + "&r=NzIweDQ4MA==", UriKind.Absolute);
        CameraAddressText.Text = "RTSP adresa kamery: " + address;
        CameraAddressEncodedText.Text = "Zakódovaná adresa kamery: " + "https://streamedian.com/embed?s=" + encoded_address + "&r=NzIweDQ4MA==";
    }

    private async void MyWebView2_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
    {
        if (args.IsSuccess)
        {
            await ((WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
        }
    }
    private void VideoView_Initialized()
    {
        const string VIDEO_URL = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
        //VideoView0.MediaPlayer = new MediaPlayer(_libvlc);
        //VideoView0.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL));

        //VideoView1.MediaPlayer = new MediaPlayer(_libvlc);
        //VideoView1.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL));

        //VideoView2.MediaPlayer = new MediaPlayer(_libvlc);
        //VideoView2.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL));

        //VideoView3.MediaPlayer = new MediaPlayer(_libvlc);
        //VideoView3.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL));

        //libvlc = new LibVLC();
        //mp = new MediaPlayer(libvlc);
        //using var media = new Media(libvlc, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
        //var vv = new VideoView
        //{
        //    MediaPlayer = mp,
        //    Width = 600,
        //    Height = 600
        //};
        //ContentArea.Children.Add(vv);
        //vv.MediaPlayer = mp;
        //mp.Play();
        //VideoView.MediaPlayer = mp;
        //VideoView.MediaPlayer.Play(media);
        //VideoView.MediaPlayer
        //mp.Play(media);
    }

}
