using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace Kamery.Core.Services;
public class ErrorService
{

    public ErrorService()
    {

    }

    public static void ThrowFileError()
    {
        throw new Exception("Nastala chyba při načítání souboru nastavení a kamer. Soubor pravděpodobně neexistuje.");
    }
}
