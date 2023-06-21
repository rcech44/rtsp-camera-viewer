using CommunityToolkit.WinUI.UI.Controls;
using Kamery.Core.Models;
using Kamery.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Kamery.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public partial class DatováMřížkaPage : Page
{
    public DatováMřížkaViewModel ViewModel
    {
        get;
    }

    public DatováMřížkaPage()
    {
        ViewModel = App.GetService<DatováMřížkaViewModel>();
        InitializeComponent();
    }

    private async void DataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
    {
        try
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                await ViewModel.Save();
                // await Database.Instance.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            DispatcherQueue.TryEnqueue(async () =>
                await new ContentDialog
                {
                    Title = "Chyba",
                    Content = $"Při editaci nastala neočekávaná chyba:\n\n{ex.Message}",
                    XamlRoot = this.XamlRoot,
                    CloseButtonText = "OK"
                }.ShowAsync()
            );
        }
    }

    private void AddCameraToGrid(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (textBoxName.Text.Length == 0 ||
            textBoxAddress.Text.Length == 0 ||
            textBoxLoginName.Text.Length == 0 ||
            textBoxLoginPassword.Text.Length == 0)
        {
            DispatcherQueue.TryEnqueue(async () =>
                await new ContentDialog
                {
                    Title = "Chyba",
                    Content = "Prosím vyplňte všechna pole",
                    XamlRoot = this.XamlRoot,
                    CloseButtonText = "OK"
                }.ShowAsync()
            );
            return;
        }
        var name = textBoxName.Text;
        var address = textBoxAddress.Text;
        var loginName = textBoxLoginName.Text;
        var loginPassword = textBoxLoginPassword.Text;
        Random random = new Random();
        var randomNumber = random.NextInt64();

        Camera _camera = new Camera { Id = randomNumber, Name = name, Address = address, LoginName = loginName, LoginPassword = loginPassword };
        ViewModel.Add(_camera );

        textBoxName.Text = "";
        textBoxAddress.Text = "";
        textBoxLoginName.Text = "";
        textBoxLoginPassword.Text = "";
        InfoBarAdd.IsOpen = true;
    }

    private async void RemoveCameraFromGrid(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedItem = DataGridCameras.SelectedItem as Camera;
        await ViewModel.Remove(selectedItem);
        InfoBarAdd.IsOpen = false;
        InfoBarDelete.IsOpen = true;
    }

    private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex()).ToString();
    }
}
