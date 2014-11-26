using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using McMDK2.Core;
using McMDK2.Core.Win32;

namespace McMDK2.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        WindowSettings WindowSettings { set; get; }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (this.WindowSettings == null)
            {
                this.WindowSettings = new WindowSettings(this);
            }

            this.WindowSettings.Reload();

            if (this.WindowSettings.Placement.HasValue)
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                var placement = this.WindowSettings.Placement.Value;
                placement.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                placement.Flags = 0;
                placement.ShowCmd = (placement.ShowCmd == (int)SW.SHOWMINIMIZED) ? (int)SW.SHOWNORMAL : placement.ShowCmd;

                NativeMethods.SetWindowPlacement(hwnd, ref placement);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                WINDOWPLACEMENT placement;
                var hwnd = new WindowInteropHelper(this).Handle;
                NativeMethods.GetWindowPlacement(hwnd, out placement);

                this.WindowSettings.Placement = placement;
                this.WindowSettings.Save();
            }
        }
    }
}
