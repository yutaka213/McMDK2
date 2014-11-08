using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK2.ViewModels
{
    public class NewProjectWindowViewModel : ViewModel
    {
        private readonly MainWindowViewModel MainWindowViewModel;

        public NewProjectWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.Templates = new ObservableCollection<ITemplate>(TemplateManager.Templates);
            this.Versions = new ObservableCollection<string>();
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
                                    this.Versions.Add((string)jobj["id"]);
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
                            this.Versions.Add((string)jobj["Version"]);
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
            { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                this.SelectedItemName = _SelectedItem.Name;
                this.SelectedItemDescription = _SelectedItem.Description;
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
                var ps = new ProgressSupporter(progress.SetText, progress.SetValue, progress.SetIndeterminate);
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

                string root;
                Stream stream;
                if (template.TemplateFile.Split(';').Length == 2)
                {
                    string path = template.TemplateFile.Split(';')[1];
                    stream = Assembly.GetAssembly(template.GetType()).GetManifestResourceStream(path);
                    root = path.Replace(".zip", "").Split('.')[path.Replace(".zip", "").Split('.').Length - 1];
                }
                else
                {
                    stream = new FileStream(template.TemplateFile, FileMode.Open, FileAccess.Read);
                    root = Path.GetFileNameWithoutExtension(template.TemplateFile);
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
                    UpdateUrl = "https://api.tuyapin.net/mcmdk/2/mods/update.json",
                    LogoFile = "",
                    Version = Define.Version,
                    Credits = String.Format("This mod generated by Minecraft Mod Development Kit {0} and other plugin creators.", Define.Version),
                    Parent = ""
                };

                var json = JsonConvert.SerializeObject(newProject, Newtonsoft.Json.Formatting.Indented);
                using (var sw = new StreamWriter(Path.Combine(newProject.Path, "project.mdk")))
                {
                    sw.WriteLine(json);
                }

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
            if (String.IsNullOrWhiteSpace(this.ProjectName) || String.IsNullOrWhiteSpace(this.ProjectVersion) || this.SelectedItem == null)
            {
                return false;
            }
            return true;
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
