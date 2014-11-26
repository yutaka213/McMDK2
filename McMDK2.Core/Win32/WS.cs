using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Win32
{
    public enum WS : long
    {
        BORDER = 0x00800000,

        CAPTION = 0x00C00000,

        CHILD = 0x40000000,

        CHILDWINDOW = CHILD,

        CLIPCHILDREN = 0x02000000,

        CLIPSIBLINGS = 0x04000000,

        DISABLED = 0x08000000,

        DLGFRAME = 0x00400000,

        GROUP = 0x00020000,

        HSCROLL = 0x00100000,

        ICONIC = 0x20000000,

        MAXIMIZE = 0x01000000,

        MAXIMIZEBOX = 0x00010000,

        MINIMIZE = ICONIC,

        MINIMIZEBOX = 0x00020000,

        OVERLAPPED = 0x00000000,

        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,

        POPUP = 0x80000000,

        POPUPWINDOW = POPUP | BORDER | SYSMENU,

        SIZEBOX = 0x00040000,

        SYSMENU = 0x00080000,

        TABSTOP = 0x00010000,

        THICKFRAME = 0x00040000,

        TILED = 0x00000000,

        TILEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,

        VISIBLE = 0x10000000,

        VSCROLL = 0x00200000
    }
}
