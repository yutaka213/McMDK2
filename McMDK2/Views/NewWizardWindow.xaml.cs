﻿using System;
using System.Collections.Generic;
using System.Linq;
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

using McMDK2.Utils.Win32;

namespace McMDK2.Views
{

    public partial class NewWizardWindow : Window
    {
        public NewWizardWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hWnd = new WindowInteropHelper(this).Handle;
            var windowStyke = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE);
            windowStyke &= ~(int)(WinApi.WindowStyles.MAXIMIZEBOX | WinApi.WindowStyles.MINIMIZEBOX);
            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, windowStyke);
        }
    }
}