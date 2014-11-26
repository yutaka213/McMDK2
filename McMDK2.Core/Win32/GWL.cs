using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Win32
{
    public enum GWL : int
    {
        WNDPROC = -4,

        HINSTANCE = -6,

        HWNDPARENT = -8,

        STYLE = -16,

        EXSTYLE = -20,

        USERDATA = -21,

        ID = -12
    }
}
