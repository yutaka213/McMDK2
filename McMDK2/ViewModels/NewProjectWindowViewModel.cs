using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core.Net;
using McMDK2.Core.Utils;
using McMDK2.Plugin;
using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin.Process;
using McMDK2.Plugin.Process.Internal;
using McMDK2.ViewModels.Dialogs;
using McMDK2.ViewModels.Internal;
using McMDK2.Views.Dialogs;

using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK2.ViewModels
{
    public class NewProjectWindowViewModel : ViewModel
    {
        private readonly MainWindowViewModel MainWindowViewModel;
        private readonly List<string> AllVersions;

        public NewProjectWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.Templates = new ObservableCollection<ITemplate>(TemplateManager.Templates);
            this.Versions = new ObservableCollection<string>();
            this.ForgeVersions = new ObservableCollection<MinecraftForge>();
            this.AllVersions = new List<string>();
            this.ProjectPath = Define.ProjectsDirectory;
        }

        public void Initialize()
        {
            Task.Run(() =>
            {
                if (Define.IsOfflineMode)
                {
                    string mcdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
                    string versionsDir = Path.Combine(mcdir, "versions", "versions.json");
                    var json = JObject.Parse((new StreamReader(versionsDir)).ReadToEnd());
                    foreach (var jobj in (JArray)json["versions"])
                    {
                        if ((string)jobj["type"] == "release")
                        {
                            if (Versioning.GetVersionNo((string)jobj["id"]) >= 125)
                            {
                                DispatcherHelper.UIDispatcher.Invoke(() =>
                                {
                                    this.AllVersions.Add((string)jobj["id"]);
                                });
                            }
                        }
                    }
                }
                else
                {
                    var json = JArray.Parse(SimpleHttp.Get(Define.ApiVersionsList));
                    foreach (var jobj in json)
                    {
                        DispatcherHelper.UIDispatcher.Invoke(() =>
                        {
                            this.AllVersions.Add((string)jobj["Version"]);
                        });
                    }
                }
            });
        }


        #region ProjectTemplates変更通知プロパティ
        private ObservableCollection<ITemplate> _Templates;

        public ObservableCollection<ITemplate> Templates
        {
            get
            { return _Templates; }
            set
            {
                if (_Templates == value)
                    return;
                _Templates = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private ITemplate _SelectedItem;

        public ITemplate SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                this.SelectedItemName = _SelectedItem.Name;
                this.SelectedItemDescription = _SelectedItem.Description;
                this.Versions.Clear();

                if (_SelectedItem.ProjectType == Define.ProjectTypeGradle)
                {
                    foreach (var version in this.AllVersions)
                    {
                        if (Versioning.GetVersionNo(version) > 1640)
                        {
                            this.Versions.Add(version);
                        }
                    }
                }
                else if (_SelectedItem.ProjectType == Define.ProjectTypeMcp)
                {
                    foreach (var version in this.AllVersions)
                    {
                        if (1320 <= Versioning.GetVersionNo(version) && Versioning.GetVersionNo(version) <= 1640)
                        {
                            this.Versions.Add(version);
                        }
                    }
                }
                else
                {
                    this.Versions = new ObservableCollection<string>(this.AllVersions);
                }

                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region SelectedItemName変更通知プロパティ
        private string _SelectedItemName;

        public string SelectedItemName
        {
            get
            { return _SelectedItemName; }
            set
            {
                if (_SelectedItemName == value)
                    return;
                _SelectedItemName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItemDescription変更通知プロパティ
        private string _SelectedItemDescription;

        public string SelectedItemDescription
        {
            get
            { return _SelectedItemDescription; }
            set
            {
                if (_SelectedItemDescription == value)
                    return;
                _SelectedItemDescription = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectName変更通知プロパティ
        private string _ProjectName;

        public string ProjectName
        {
            get
            { return _ProjectName; }
            set
            {
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                var projectPath = Path.Combine(Define.ProjectsDirectory, _ProjectName);
                int i = 1;
                while (FileController.Exists(projectPath))
                {
                    projectPath = Path.Combine(Define.ProjectsDirectory, _ProjectName + "_" + (i++));
                }
                this.ProjectPath = projectPath;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region ProjectPath変更通知プロパティ
        private string _ProjectPath;

        public string ProjectPath
        {
            get
            { return _ProjectPath; }
            set
            {
                if (_ProjectPath == value)
                    return;
                _ProjectPath = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectVersion変更通知プロパティ
        private string _ProjectVersion;

        public string ProjectVersion
        {
            get
            { return _ProjectVersion; }
            set
            {
                if (_ProjectVersion == value)
                    return;
                _ProjectVersion = value;
                this.ForgeVersions.Clear();

                Task.Run(() =>
                {
                    var json = JArray.Parse(SimpleHttp.Get(String.Format("https://api.tuyapin.net/mcmdk/2/forge/version/{0}.json", _ProjectVersion)));
                    foreach (var jobj in json)
                    {
                        DispatcherHelper.UIDispatcher.Invoke(() =>
                        {
                            this.ForgeVersions.Add(JsonConvert.DeserializeObject<MinecraftForge>(jobj.ToString()));
                        });
                    }
                });

                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region Versions変更通知プロパティ
        private ObservableCollection<string> _Versions;

        public ObservableCollection<string> Versions
        {
            get
            { return _Versions; }
            set
            {
                if (_Versions == value)
                    return;
                _Versions = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ForgeVersion変更通知プロパティ
        private MinecraftForge _ForgeVersion;

        public MinecraftForge ForgeVersion
        {
            get
            { return _ForgeVersion; }
            set
            {
                if (_ForgeVersion == value)
                    return;
                _ForgeVersion = value;
                RaisePropertyChanged();
                this.OKCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region ForgeVersions変更通知プロパティ
        private ObservableCollection<MinecraftForge> _ForgeVersions;

        public ObservableCollection<MinecraftForge> ForgeVersions
        {
            get
            { return _ForgeVersions; }
            set
            {
                if (_ForgeVersions == value)
                    return;
                _ForgeVersions = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OKCommand
        private ViewModelCommand _OKCommand;

        public ViewModelCommand OKCommand
        {
            get
            {
                if (_OKCommand == null)
                {
                    _OKCommand = new ViewModelCommand(OK, CanOK);
                }
                return _OKCommand;
            }
        }

        //TODO: Refactoring
        public void OK()
        {
            this.MainWindowViewModel.CloseProject();

            bool Cancel = false;
            var progress = new ProgressDialogViewModel();
            progress.SetIndeterminate(true);
            progress.SetText("プロジェクトを取り出しています...");
            // Actionは非同期で行われます。
            // UI要素を変更する際は、Dispatcherを使用してください。
            progress.Action += () =>
            {
                var newProject = new Project
                {
                    Path = this.ProjectPath,
                    Name = this.ProjectName,
                    Id = Guid.NewGuid().ToString()
                };

                FileController.CreateDirectory(newProject.Path);

                var template = this.SelectedItem;
                var ps = new ProgressSupporter(progress.SetText, progress.SetValue, progress.SetIndeterminate, this.MainWindowViewModel.SetTaskText);
                var im = new IndirectlyMessenger(this.Messenger);
                var wts = new WindowTransitionSupporter(im.Raise, im.Raise, im.RaiseAsync, im.RaiseAsync);

                // Send Pre initialization event
                var pre = new PreInitializationArgs(newProject.Path, newProject.UserProperties, wts, this.ProjectVersion, ps);
                template.PreInitialization(pre);
                if (pre.Cancel)
                {
                    FileController.Delete(newProject.Path);
                    Cancel = pre.Cancel;
                    progress.Close();
                    return;
                }
                // Handled
                this.SetupProject(ps);
                this.ExtractProject(newProject, ps);

                string rootpath = newProject.Path + "\\";
                // Loading MINECRAFT MOD PROJECT(*.MMPROJ) file.
                var element = XElement.Load(Path.Combine(newProject.Path, newProject.Name + ".mmproj"));
                var q = from p in element.Element("Items").Elements()
                        select new
                        {
                            Include = p.Attribute("Include").Value,
                            Id = p.Attribute("Id") == null ? Guid.NewGuid().ToString() : p.Attribute("Id").Value
                        };

                // Send initialization event
                var init = new InitializationArgs(newProject.Path, q.Select(item => item.Include).ToList().AsReadOnly(), wts, ps);
                template.Initialization(init);
                if (init.Cancel)
                {
                    FileController.Delete(newProject.Path);
                    Cancel = pre.Cancel;
                    progress.Close();
                    return;
                }
                // Handled

                foreach (var item in q)
                {
                    string[] path = item.Include.Split('\\');
                    ObservableCollection<ProjectItem> cur = newProject.Items;
                    for (int i = 0; i < path.Length; i++)
                    {
                        if (path.Length - 1 == 0)
                        {
                            newProject.Items.Add(new ProjectItem
                            {
                                Name = path[0],
                                FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[0])),
                                FilePath = rootpath + item.Include,
                                Id = item.Id
                            });
                        }
                        else
                        {
                            // do not FILE
                            if (i != path.Length - 1)
                            {
                                var sb = new StringBuilder();
                                for (int j = 0; j < i + 1; j++)
                                {
                                    sb.Append(path[j]);
                                    sb.Append("\\");
                                }
                                sb.Remove(sb.Length - 1, 1);

                                // nf directory
                                if (cur.SingleOrDefault(w => w.FilePath == rootpath + sb.ToString()) == null)
                                {
                                    cur.Add(new ProjectItem
                                    {
                                        Name = path[i],
                                        FileType = Define.IdentifierDirectory,
                                        FilePath = rootpath + sb.ToString()
                                    });
                                }
                                cur = cur.Single(w => w.FilePath == rootpath + sb.ToString()).Children;
                            }
                            // FILE
                            else
                            {
                                cur.Add(new ProjectItem
                                {
                                    Name = path[i],
                                    FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[i])),
                                    FilePath = rootpath + item.Include,
                                    Id = item.Id
                                });
                            }
                        }
                    }
                }

                // Send post initialization event
                var post = new PostInitializationArgs(newProject.Path, wts, ps);
                template.PostInitialization(post);
                if (post.Cancel)
                {
                    FileController.Delete(newProject.Path);
                    Cancel = pre.Cancel;
                    progress.Close();
                    return;
                }

                // Handled
                progress.Close();
                ps.SetTaskText("準備完了");

                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    this.MainWindowViewModel.Title = this.ProjectName + " - Minecraft Mod Development Kit";
                    this.MainWindowViewModel.IsLoadedProject = true;
                    this.MainWindowViewModel.CurrentProject = newProject;
                    this.MainWindowViewModel.RecentProjects.Add(newProject);

                    var tab = this.MainWindowViewModel.Tabs.SingleOrDefault(w => (string)w.Tag /* Suppress warning CS0253 */== Guids.StartPageGuid);
                    if (tab != null)
                        this.MainWindowViewModel.Tabs.Remove(tab);
                });
            };
            //Messenger.Raise(new TransitionMessage(progress, "ProgressDialog"));
            Messenger.Raise(new TransitionMessage(typeof(ProgressDialog), progress, TransitionMode.Modal, "Transition"));

            // If handeld Cancel Handler, do not work progress.
            if (!Cancel)
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));

        }

        public bool CanOK()
        {
            if (String.IsNullOrWhiteSpace(this.ProjectName) || String.IsNullOrWhiteSpace(this.ProjectVersion) || this.SelectedItem == null || this.ForgeVersion == null)
            {
                return false;
            }
            return true;
        }

        private void ExtractProject(Project newProject, ProgressSupporter progress)
        {
            string root;
            Stream stream;
            var template = this.SelectedItem;
            if (template.TemplateFile.StartsWith("assembly;"))
            {
                string path = template.TemplateFile.Split(';')[1];
                stream = Assembly.GetAssembly(template.GetType()).GetManifestResourceStream(path);
                root = path.Replace(".zip", "").Split('.')[path.Replace(".zip", "").Split('.').Length - 1];
            }
            else
            {
                string path = template.TemplateFile.Replace("file;", "");
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                root = Path.GetFileNameWithoutExtension(path);
            }
            var fs = new FileStream(Path.Combine(newProject.Path, "Template.zip"), FileMode.Create, FileAccess.Write);
            int nbyte;
            while ((nbyte = stream.ReadByte()) != -1)
            {
                fs.WriteByte((byte)nbyte);
            }
            fs.Close();
            stream.Close();

            progress.SetText("展開しています...");

            ZipFile.ExtractToDirectory(Path.Combine(newProject.Path, "Template.zip"), newProject.Path);
            FileController.Delete(Path.Combine(newProject.Path, "Template.zip"));
            FileController.Rename(Path.Combine(newProject.Path, root + ".mmproj"), Path.Combine(newProject.Path, newProject.Name + ".mmproj"));

            // Set mcmod.info
            newProject.ProjectSettings["modinfo"] = new ModInfo
            {
                ModId = Guid.NewGuid().ToString(),
                Name = newProject.Name,
                Description = "",
                Url = "",
                UpdateUrl = "https://api.mcmdk.tk/mods/update.json",
                LogoFile = "",
                Version = Define.Version,
                Credits = String.Format("This mod generated by Minecraft Mod Development Kit {0} and other plugin creators.", Define.Version),
                Parent = ""
            };
            newProject.ProjectSettings["mcversion"] = this.ProjectVersion;
            newProject.ProjectSettings["buildtype"] = this.SelectedItem.ProjectType;

            var json = JsonConvert.SerializeObject(newProject, Newtonsoft.Json.Formatting.Indented);
            using (var sw = new StreamWriter(Path.Combine(newProject.Path, "project.mdk")))
            {
                sw.WriteLine(json);
            }

        }

        private void SetupProject(ProgressSupporter progress)
        {
            var forge = this.ForgeVersion;
            progress.SetIsIndetermiate(false);
            progress.SetText("必要ファイルをを取得しています...");

            // ReSharper disable AssignNullToNotNullAttribute
            if (!FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileName(forge.SrcUri))))
                DownloadFile(forge.SrcUri, progress).Wait();
            if (!FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri))))
                ExtractFile(
                    Path.Combine(Define.CacheDirectory, Path.GetFileName(forge.SrcUri)),
                    Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri)),
                    progress).Wait();

            if (FileController.Exists(Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri), "gradlew")))
            {
                progress.SetIsIndetermiate(true);
                var sb = new StringBuilder();
                sb.AppendLine("設定を行っています。");
                sb.AppendLine("しばらくお待ちください...");
                progress.SetText(sb.ToString());
                // Gradlew
                var proc = new Process();
                proc.StartInfo.FileName = Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri), "gradlew.bat");
                proc.StartInfo.WorkingDirectory = Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri));
                proc.StartInfo.Arguments = "setupDevWorkspace eclipse";
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.OutputDataReceived += (a, b) =>
                {
                    progress.SetTaskText("Gradle Task -> " + b.Data);
                    Define.GetLogger().Info(b.Data);
                };
                proc.ErrorDataReceived += (a, b) => Define.GetLogger().Error(b.Data);
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();

                FileController.Delete(Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri), "src", "main", "java", "com"));
            }
            else
            {
                // MCP
                string json = DownloadString(String.Format("https://api.tuyapin.net/mcmdk/2/mcp/version/{0}.json", forge.MinecraftVersion));
                var o = JArray.Parse(json);
                string target = ((string)((JObject)o.Last)["Version"]).Replace(".", "");
                DownloadFile(String.Format("http://mcp.ocean-labs.de/files/archive/mcp{0}.zip", target), progress).Wait();
                ExtractFile(
                    Path.Combine(Define.CacheDirectory, Path.GetFileName(String.Format("mcp{0}.zip", target))),
                    Path.Combine(Define.CacheDirectory, Path.GetFileNameWithoutExtension(forge.SrcUri)),
                    progress).Wait();

                // TODO: MCP Setup
            }

        }

        private async Task DownloadFile(string uri, ProgressSupporter s)
        {
            s.SetProgressValue(0);
            s.SetIsIndetermiate(false);
            s.SetTaskText("ファイルをダウンロードしています... -> " + uri);
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
                    s.SetTaskText("ファイルを展開しています... -> " + entry.FullName);
                }
            });
        }
        #endregion


        #region CancelCommand
        private ViewModelCommand _CancelCommand;

        public ViewModelCommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new ViewModelCommand(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
        }
        #endregion

    }
}
