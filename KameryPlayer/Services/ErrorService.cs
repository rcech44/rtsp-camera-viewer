using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace KameryPlayer.Services;
public class ErrorService
{

    public ErrorService()
    {

    }

    public static void ThrowFileError()
    {
        MessageBox.Show("Nastala chyba při načítání souboru nastavení a kamer. Program se nyní ukončí.");
        System.Environment.Exit(1);
    }
}
