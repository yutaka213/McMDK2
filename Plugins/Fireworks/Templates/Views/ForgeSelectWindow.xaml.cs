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
using McMDK2.Core.Win32;

namespace Fireworks.Templates.Views
{
    /* 
     * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// ForgeSelectWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ForgeSelectWindow : Window
    {
        public ForgeSelectWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hWnd = new WindowInteropHelper(this).Handle;
            var windowStyle = NativeMethods.GetWindowLong(hWnd, (int)GWL.STYLE);
            windowStyle &= ~(int)(WS.MAXIMIZEBOX | WS.MINIMIZEBOX | WS.SYSMENU);
            NativeMethods.SetWindowLong(hWnd, (int)GWL.STYLE, windowStyle);
        }
    }
}