using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fireworks.Templates.ViewModels;
using Fireworks.Templates.Views;
using McMDK2.Core;
using McMDK2.Core.Utils;
using McMDK2.Plugin;
using McMDK2.Plugin.Process;
using McMDK2.Plugin.Process.Internal;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;

// ReSharper disable AssignNullToNotNullAttribute
namespace Fireworks.Templates
{
    /// <summary>
    /// McMDK2 の標準的なプロジェクトのテンプレート
    /// </summary>
    public class StandardTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(Minecraft Forge ユニバーサル)"; }
        }

        public string Id
        {
            get { return "43ECD3FD-7E29-4968-AF55-4C5ED437E7B3"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return Id + ";Fireworks.Resources.Contract_32xLG.png"; }
        }

        public string Description
        {
            get { return "Minecraft Forge Universalを前提Modとした、Modの作成を行うためのテンプレートです。" + Environment.NewLine + "Minecraft 1.3.2以降を対象としたModを作成することができます。"; }
        }

        public string TemplateFile
        {
            get { return Id + ";Fireworks.Templates.StandardTemplate.zip"; }
        }

        #region PreInitialization
        public void PreInitialization(PreInitializationArgs args)
        {
            // If Minecraft version is older than 1.3.2, Return process to New Project Wizard.
            if (Versioning.GetVersionNo(args.MinecraftVersion) < 132)
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "Invalid Argument";
                taskDialog.InstructionText = "不正なバージョンです。";
                taskDialog.Text =
@"Minecraftのバージョンに、1.3.2より前のものが指定されています。
このテンプレートでは、Minecraft 1.3.2以降のバージョンにしか対応していません。
バージョンを1.3.2以上に設定しなおしてから、もう一度試してください。";
                taskDialog.Icon = TaskDialogStandardIcon.Information;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (_sender, _e) =>
                {
                    ((TaskDialog)_sender).Icon = ((TaskDialog)_sender).Icon;
                };
                taskDialog.Show();
                args.Cancel = true;
                return;
            }

            var vm = new ForgeSelectWindowViewModel(args.MinecraftVersion, args.UserProperties);
            args.WindowTransition.Raise(typeof(ForgeSelectWindow), vm, "Modal");

            var forge = vm.SelectedVersion;
            args.ProgressWindow.SetIsIndetermiate(false);
            args.ProgressWindow.SetText("必要ファイルを取得しています...");
            if (!FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileName(forge.SrcUri))))
                DownloadFile(forge.SrcUri, args.ProgressWindow).Wait();
            if (!FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri))))
                ExtractFile(
                    Path.Combine(Define.CacheDirectory, Path.GetFileName(forge.SrcUri)),
                    Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri)),
                    args.ProgressWindow).Wait();

            if (FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri), "gradlew")))
            {
                args.ProgressWindow.SetIsIndetermiate(true);
                var sb = new StringBuilder();
                sb.AppendLine("設定を行っています。");
                sb.AppendLine("しばらくお待ちください...");
                args.ProgressWindow.SetText(sb.ToString());
                args.UserProperties.Add("Mode", "Gradle");
                // Gradlew
                var proc = new Process();
                proc.StartInfo.FileName = Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri), "gradlew.bat");
                proc.StartInfo.WorkingDirectory = Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri));
                proc.StartInfo.Arguments = "setupDevWorkspace eclipse";
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.OutputDataReceived += (a, b) => Define.GetLogger().Info(b.Data);
                proc.ErrorDataReceived += (a, b) => Define.GetLogger().Error(b.Data);
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            }
            else
            {
                args.UserProperties.Add("Mode", "Mcp");
                // MCP
                string json = DownloadString(String.Format("https://api.tuyapin.net/mcmdk/2/mcp/version/{0}.json", forge.MinecraftVersion));
                var o = JArray.Parse(json);
                string target = ((string)((JObject)o.Last)["Version"]).Replace(".", "");
                DownloadFile(String.Format("http://mcp.ocean-labs.de/files/archive/mcp{0}.zip", target), args.ProgressWindow).Wait();
                ExtractFile(
                    Path.Combine(Define.CacheDirectory, Path.GetFileName(String.Format("mcp{0}.zip", target))),
                    Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri)),
                    args.ProgressWindow).Wait();


            }
        }

        private async Task DownloadFile(string uri, ProgressSupporter s)
        {
            s.SetProgressValue(0);
            s.SetIsIndetermiate(false);
            var client = new WebClient();
            client.DownloadProgressChanged += (a, b) => s.SetProgressValue(b.ProgressPercentage);
            await client.DownloadFileTaskAsync(new Uri(uri), Path.Combine(Define.CacheDirectory, Path.GetFileName(uri)));
        }

        private string DownloadString(string uri)
        {
            var client = new WebClient();
            return client.DownloadString(new Uri(uri));
        }

        private async Task ExtractFile(string path1, string path2, ProgressSupporter s)
        {
            await Task.Run(() =>
            {
                ZipArchive archive = ZipFile.OpenRead(path1);
                foreach (var entry in archive.Entries)
                {
                    FileController.CreateDirectory(Path.GetDirectoryName(Path.Combine(path2, entry.FullName)));
                    if (entry.FullName.EndsWith("/"))
                        continue;
                    entry.ExtractToFile(Path.Combine(path2, entry.FullName));
                }
            });
        }

        #endregion

        public void Initialization(InitializationArgs args)
        {
            //
        }

        public void PostInitialization(PostInitializationArgs args)
        {
            //
        }
    }
}
