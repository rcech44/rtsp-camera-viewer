using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamery.Core.Models;
public class CameraPlayerSettings
{
    public bool Fullscreen
    {
        get; set; 
    }
    public bool Refresh
    {
        get; set; 
    }
    public int RefreshInterval
    {
        get; set;
    }
    public bool Volume
    {
        get; set;
    }
    public int NumberOfRows
    {
        get; set; 
    }
    public int NumberOfCols
    {
        get; set;
    }
}
