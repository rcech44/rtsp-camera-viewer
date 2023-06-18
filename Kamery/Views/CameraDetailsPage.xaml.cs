using CommunityToolkit.WinUI.UI.Controls;

using Kamery.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Kamery.Views;

public sealed partial class CameraDetailsPage : Page
{
    public CameraDetailsViewModel ViewModel
    {
        get;
    }

    public CameraDetailsPage()
    {
        ViewModel = App.GetService<CameraDetailsViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
