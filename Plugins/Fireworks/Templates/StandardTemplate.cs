using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fireworks.Templates.ViewModels;
using Fireworks.Templates.Views;
using McMDK2.Core;
using McMDK2.Plugin;
using McMDK2.Plugin.Process;
using McMDK2.Plugin.Process.Internal;

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
                // Gradlew
            }
            else
            {
                // MCP
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
