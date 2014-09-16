using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

using Livet;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using McMDK2.Core;
using McMDK2.Core.Minecaft;
using McMDK2.Core.Plugin;

using Microsoft.WindowsAPICodePack.Dialogs;

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

            // Checking Account Information
            // あくまで簡易的なもの
#if !DEBUG
            #region MINECRAFT ACCOUNT CHECK
            string mcdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft"; /* %appdata%/.minecraft */
            if (!FileController.Exists(mcdir + "\\launcher_profiles.json"))
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "Error";
                taskDialog.InstructionText = ".minecraft を確認できませんでした。";
                taskDialog.Text = "Minecraft Launcherを一度起動させて、ログインしてから、もう一度試してください。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (_sender, _e) =>
                {
                    ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                };
                taskDialog.Show();

                Environment.Exit(0);
            }
            using (var reader = File.OpenText(mcdir + "\\launcher_profiles.json"))
            {
                var o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                if (o["selectedUser"] == null)
                {
                    var taskDialog = new TaskDialog();
                    taskDialog.Caption = "Error";
                    taskDialog.InstructionText = "Profile を確認できませんでした。";
                    taskDialog.Text = "Minecraft Launcherでログインしてから、もう一度試してください。";
                    taskDialog.Icon = TaskDialogStandardIcon.Error;
                    taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                    taskDialog.Opened += (_sender, _e) =>
                    {
                        ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                    };
                    taskDialog.Show();

                    Environment.Exit(0);
                }

                string uuid = (string)o["selectedUser"];
                Session session = new Session();
                Session.User profile;
                try
                {
                    profile = session.GetUserProfileFromUUID(uuid);
                }
                catch (Exception)
                {
                    profile = null;
                }
                if (ValueChecker.IsNull(profile))
                {
                    var taskDialog = new TaskDialog();
                    taskDialog.Caption = "Error";
                    taskDialog.InstructionText = "不正な UUID を検出しました。";
                    taskDialog.Text = "Minecraft Launcher に登録されているユーザーのUUIDが不正です。再度ログインした後、もう一度試してください。";
                    taskDialog.Icon = TaskDialogStandardIcon.Error;
                    taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                    taskDialog.Opened += (_sender, _e) =>
                    {
                        ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                    };
                    taskDialog.Show();

                    Environment.Exit(0);
                }
                Define.GetLogger().Info("Loggined : " + profile.Name);
            }
            #endregion
#endif
            // Creating some directories.
            FileController.CreateDirectory(Define.AssetsDirectory);
            FileController.CreateDirectory(Define.CacheDirectory);
            FileController.CreateDirectory(Define.LogDirectory);
            FileController.CreateDirectory(Define.PluginDirectory);
            FileController.CreateDirectory(Define.ProjectsDirectory);

            // Register Defaults
            ItemManager.Register("png", "Image", null, "pack://application:,,,/Resources/Image_24x.png");
            ItemManager.Register("ogg", "Sound", null, "pack://application:,,,/Resources/Soundfile_461.png");
            ItemManager.Register("txt", "Text", null, "pack://application:,,,/Resources/Textfile_818_16x.png");
            ItemManager.Register("lang", "Text", null, "pack://application:,,,/Resources/Textfile_818_16x.png");
            ItemManager.Register("json", "Text", null, "pack://application:,,,/Resources/Textfile_818_16x.png");
            ItemManager.Register("java", "Java", null, "pack://application:,,,/Resources/Textfile_818_16x.png");
            ItemManager.Register("", "Directory", null, "pack://application:,,,/Resources/Folder_6222.png");


            // Load Plugins from PluginDirectory.
            PluginLoader.Load();

            // Checking Updates
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
