using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Livet;

using McMDK2.Core;
using McMDK2.Core.Plugin;

namespace McMDK2
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //
            FileController.CreateDirectory(Define.AssetsDirectory);
            FileController.CreateDirectory(Define.CacheDirectory);
            FileController.CreateDirectory(Define.LogDirectory);
            FileController.CreateDirectory(Define.PluginDirectory);
            FileController.CreateDirectory(Define.ProjectsDirectory);

            PluginLoader.Load();
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO:ロギング処理など
            MessageBox.Show(
                "不明なエラーが発生しました。アプリケーションを終了します。",
                "エラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Console.WriteLine(e.ExceptionObject);

            Environment.Exit(1);
        }
    }
}
