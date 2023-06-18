using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Kamery.Helpers;


public class Base64Converter
{
    public Base64Converter()
    {
    }

    public string Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
