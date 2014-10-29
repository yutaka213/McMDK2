using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

using Livet;
using McMDK2.Core.Net;
using McMDK2.ViewModels.TabPages;
using McMDK2.Views.TabPages;
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

#if !DEBUG
            // Checking Account Information
            // あくまで簡易的なもの
            #region MINECRAFT ACCOUNT CHECK
            if (!(bool)Define.GetSettings().Settings["IsCheckedAccount"])
            {
                string mcdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft"); /* %appdata%/.minecraft */
                if (!FileController.Exists(Path.Combine(mcdir, "launcher_profiles.json")))
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
                using (var reader = File.OpenText(Path.Combine(mcdir, "launcher_profiles.json")))
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
                    Define.GetSettings().Settings["IsCheckedAccount"] = true;
                }

            }
            #endregion
#endif
            // Checking New Version
            try
            {
                string response = SimpleHttp.Get(Define.ApiUpdate);
                var json = JObject.Parse(response);
                if (int.Parse(Define.Version.Replace(".", "")) < int.Parse(((string)json["McMDK2"]["version"]).Replace(".", "")))
                {
                    // New version found
                    bool force = bool.Parse((string)json["McMDK2"]["force_update"]);
                    Define.FoundNewVersion = true;
                    if (force)
                    {
                        var taskDialog = new TaskDialog();
                        taskDialog.Caption = "Update";
                        taskDialog.InstructionText = "McMDKの更新があります。";
                        taskDialog.Text = "現在ご利用しているバージョンよりも、新しいバージョンが公開されています。";
                        taskDialog.DetailsExpandedText = String.Format(
                            "現在使用中のバージョン　　：{0}" + Environment.NewLine +
                            "現在使用可能なバージョン ：{1}",
                            Define.Version,
                            (string)json["McMDK2"]["version"]);
                        taskDialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandContent;
                        taskDialog.DetailsExpanded = false;
                        taskDialog.Icon = TaskDialogStandardIcon.Information;
                        taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                        taskDialog.Opened += (_sender, _e) =>
                        {
                            ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                        };
                        taskDialog.Show();

                        //以下更新
                    }
                    else
                    {
                        var taskDialog = new TaskDialog();
                        taskDialog.Caption = "Update";
                        taskDialog.InstructionText = "McMDKの更新があります。";
                        taskDialog.Text = "現在ご利用しているバージョンよりも、新しいバージョンが公開されています。" + Environment.NewLine +
                                          "最新版へと更新を行いますか？";
                        taskDialog.DetailsExpandedText = String.Format(
                            "現在使用中のバージョン　　：{0}" + Environment.NewLine +
                            "現在使用可能なバージョン ：{1}",
                            Define.Version,
                            (string)json["McMDK2"]["version"]);
                        taskDialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandContent;
                        taskDialog.DetailsExpanded = false;
                        taskDialog.Icon = TaskDialogStandardIcon.Information;
                        taskDialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                        taskDialog.Opened += (_sender, _e) =>
                        {
                            ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                        };
                        if (taskDialog.Show() == TaskDialogResult.Yes)
                        {
                            //以下更新
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                {
                    // cannot resolved domain 'api.tuyapin.net'.

                }
            }


            // Creating some directories.
            FileController.CreateDirectory(Define.AssetsDirectory);
            FileController.CreateDirectory(Define.CacheDirectory);
            FileController.CreateDirectory(Define.LogDirectory);
            FileController.CreateDirectory(Define.PluginDirectory);
            FileController.CreateDirectory(Define.ProjectsDirectory);

            // Default
            // XML FORMAT
#pragma warning disable 612
            ItemManager.RegisterExtension("mod", "Mod", new ModdingPage(), new ModdingPageViewModel());
            ItemManager.RegisterIcon("Mod", "pack://application:,,,/Resources/ASCube_16xLG.png");
            ItemManager.RegisterExtension("", "DIRECTORY", null);// Preview unavailable
            ItemManager.RegisterIcon("DIRECTORY", "pack://application:,,,/Resources/Folder_6222.png");


            // Load Plugins from PluginDirectory.
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
