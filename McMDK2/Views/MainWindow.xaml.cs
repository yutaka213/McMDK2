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
                placement.length = Marshal.SizeOf(typeof(WinApi.WINDOWPLACEMENT));
                placement.flags = 0;
                placement.showCmd = (placement.showCmd == 2/*SW_SHOWMINIMIZED*/) ? 1/*SW_SHOWNORMAL*/ : placement.showCmd;

                WinApi.SetWindowPlacement(hwnd, ref placement);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                WinApi.WINDOWPLACEMENT placement;
                var hwnd = new WindowInteropHelper(this).Handle;
                WinApi.GetWindowPlacement(hwnd, out placement);

                this.WindowSettings.Placement = placement;
                this.WindowSettings.Save();
            }
        }
    }
}
