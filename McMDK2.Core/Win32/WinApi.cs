using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable 1591

namespace McMDK2.Core.Win32
{
    /// <summary>
    /// For Windows 32 APIs.
    /// </summary>
    public static class WinApi
    {

        #region Window Place

        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        #region RECT struct
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }
        #endregion

        #region POINT struct
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        #endregion

        #region WINDOWPLACEMENT struct
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT minPosition;
            public POINT maxPosition;
            public RECT normalPosition;
        }
        #endregion

        #endregion

        #region Window Style

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

        public enum WindowStyles
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

            //POPUP = 0x80000000,

            //POPUPWINDOW = POPUP | BORDER | SYSMENU,

            SIZEBOX = 0x00040000,

            SYSMENU = 0x00080000,

            TABSTOP = 0x00010000,

            THICKFRAME = 0x00040000,

            TILED = 0x00000000,

            TILEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,

            VISIBLE = 0x10000000,

            VSCROLL = 0x00200000,
        }

        public static readonly int GWL_STYLE = -16;

        #endregion
    }
}
