using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Models;
using McMDK2.Plugin;
using McMDK2.ViewModels.Dialogs;
using McMDK2.Views.TabPages;
using McMDK2.ViewModels.TabPages;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK2.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public ApplicationInternalSettings internalSettings;

        public MainWindowViewModel()
        {
            this.Tabs = new ObservableCollection<TabItem>();
            this.RecentProjects = new ObservableCollection<Project>();
            this.IsLoadedProject = false;
            this.TaskText = "準備完了";
            this.SelectedTabIndex = -1;

            if (this.internalSettings == null)
            {
                this.internalSettings = new ApplicationInternalSettings();
            }

            this.internalSettings.Reload();
            if (this.internalSettings.RecentProjects != null)
            {
                foreach (var item in this.internalSettings.RecentProjects)
                {
                    if (item != null)
                        this.RecentProjects.Add(item);
                }
            }
        }

        public void Initialize()
        {
            var startPage = new TabItem
            {
                Header = "Start",
                Content = new StartPage { DataContext = new StartPageViewModel(this) }
            };
            this.Tabs.Add(startPage);
            this.SelectedTabIndex = 0;
        }

        public void Uninitialize()
        {
            //TODO: Check
            var items = new Project[5];
            int length = this.RecentProjects.Count > 5 ? this.RecentProjects.Count - 5 : 0;
            for (int i = this.RecentProjects.Count - 1, j = 0; i >= length; i--, j++)
            {
                items[j] = this.RecentProjects[i];
            }

            this.internalSettings.RecentProjects = items;
            this.internalSettings.Save();
        }


        #region NewWizardCommand
        private ViewModelCommand _NewWizardCommand;

        public ViewModelCommand NewWizardCommand
        {
            get
            {
                if (_NewWizardCommand == null)
                {
                    _NewWizardCommand = new ViewModelCommand(NewWizard);
                }
                return _NewWizardCommand;
            }
        }

        public void NewWizard()
        {
            Messenger.Raise(new TransitionMessage(new NewWizardWindowViewModel(this), "ShowNewWizard"));
        }
        #endregion


        #region OpenProjectCommand
        private ViewModelCommand _OpenProjectCommand;

        public ViewModelCommand OpenProjectCommand
        {
            get
            {
                if (_OpenProjectCommand == null)
                {
                    _OpenProjectCommand = new ViewModelCommand(OpenProject);
                }
                return _OpenProjectCommand;
            }
        }

        public void OpenProject()
        {
            var ofd = new OpenFileDialog
            {
                FileName = "",
                Filter = "McMDK Main Project File (*.mdk)|*.mdk"
            };
            if (ofd.ShowDialog() == true)
            {
                if (FileController.Exists(ofd.FileName))
                {
                    var progress = new ProgressDialogViewModel();
                    progress.SetIndeterminate(true);
                    progress.SetText("プロジェクトを読み込んでいます...");
                    progress.Action += () =>
                    {
                        string file = ofd.FileName;
                        string json;
                        using (var sr = new StreamReader(file))
                        {
                            json = sr.ReadToEnd();
                        }
                        var obj = JsonConvert.DeserializeObject<Project>(json);
                        obj.Items = new ObservableCollection<ProjectItem>(); // Items is always clear.
                        string rootpath = obj.Path + "\\";

                        // Loading MINECRAFT MOD PROJECT(*.mmproj) file.
                        var element = XElement.Load(obj.Path + "//" + obj.Name + ".mmproj");
                        var q = from p in element.Element("Items").Elements()
                                select new
                                {
                                    Include = p.Attribute("Include").Value
                                };

                        foreach (var item in q)
                        {
                            string[] path = item.Include.Split('\\');
                            ObservableCollection<ProjectItem> cur = obj.Items;
                            for (int i = 0; i < path.Length; i++)
                            {
                                if (path.Length - 1 == 0)
                                {
                                    obj.Items.Add(new ProjectItem
                                    {
                                        Name = path[0],
                                        FileType = ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[0])),
                                        FilePath = rootpath + item.Include
                                    });
                                }
                                else
                                {
                                    if (i != path.Length - 1)
                                    {
                                        if (cur.SingleOrDefault(w => w.Name == path[i]) == null)
                                        {
                                            cur.Add(new ProjectItem
                                            {
                                                Name = path[i],
                                                FileType = "DIRECTORY",
                                                FilePath = rootpath + item.Include
                                            });
                                        }
                                        cur = cur.Single(w => w.Name == path[i]).Children;
                                    }
                                    else
                                    {
                                        cur.Add(new ProjectItem
                                        {
                                            Name = path[i],
                                            FileType =
                                                ItemManager.GetIdentifierFromExtension(Path.GetExtension(path[i])),
                                            FilePath = rootpath + item.Include
                                        });
                                    }
                                }
                            }
                        }

                        this.CurrentProject = obj;
                        progress.Close();
                        var tab = this.Tabs.SingleOrDefault(w => (string)w.Header == "Start");
                        if (tab != null)
                            this.Tabs.Remove(tab);

                        this.RecentProjects.Add(obj);
                    };
                    Messenger.Raise(new TransitionMessage(progress, "ProgressDialog"));

                }
                else
                {
                    System.Windows.MessageBox.Show("ファイルを開けませんでした。");
                }
            }
        }
        #endregion


        #region CloseCommand
        private ListenerCommand<object> _CloseCommand;

        public ListenerCommand<object> CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new ListenerCommand<object>(Close);
                }
                return _CloseCommand;
            }
        }

        public void Close(object parameter)
        {
            object content = ((TabItem)parameter).Content;
            if (content != null)
            {
                var view = content as ItemView;
                if (view != null)
                {
                    view.Closing();
                }
                else if (((UserControl)content).DataContext != null && ((UserControl)content).DataContext is ItemViewEx)
                {
                    ((ItemViewEx)((UserControl)content).DataContext).Closing();
                }
            }
            this.Tabs.Remove((TabItem)parameter);
        }
        #endregion


        #region CloseAppCommand
        private ViewModelCommand _CloseAppCommand;

        public ViewModelCommand CloseAppCommand
        {
            get
            {
                if (_CloseAppCommand == null)
                {
                    _CloseAppCommand = new ViewModelCommand(CloseApp);
                }
                return _CloseAppCommand;
            }
        }

        public void CloseApp()
        {
            Environment.Exit(0);
        }
        #endregion


        #region MouseDoubleClickCommand
        private ListenerCommand<object> _MouseDoubleClickCommand;

        public ListenerCommand<object> MouseDoubleClickCommand
        {
            get
            {
                if (_MouseDoubleClickCommand == null)
                {
                    _MouseDoubleClickCommand = new ListenerCommand<object>(MouseDoubleClick);
                }
                return _MouseDoubleClickCommand;
            }
        }

        // プロジェクトエクスプローラーのItemがWクリック(=選択)された際にコールされます。
        public void MouseDoubleClick(object parameter)
        {
            var item = parameter as ProjectItem;
            if (item.FileType == "DIRECTORY")
                return;

            if (this.Tabs.SingleOrDefault(w => (string)w.Header == item.Name) != null)
            {
                //Open Tab
                this.SelectedTabIndex = this.Tabs.IndexOf(this.Tabs.Single(w => (string)w.Header == item.Name));
                return;
            }

            var newtab = new TabItem { Header = item.Name };
            var view = ItemManager.GetItemViewFromExtension(Path.GetExtension(item.Name));
            if (view == null)
            {
                // Preview unavailable
                var page = new NullPage();
                var vm = new NullPageViewModel();
                vm.Message = String.Format("この項目({0})ではプレビュー及び編集機能は使用できません。", item.Name);
                page.DataContext = vm;
                newtab.Content = page;
            }
            else
            {
                var itemView = view as ItemView;
                if (itemView != null)
                {
                    itemView.Initialize(item.FilePath);
                }
                else if (view.DataContext != null && view.DataContext is ItemViewEx)
                {
                    ((ItemViewEx)view.DataContext).Initialize(item.FilePath);
                }
                newtab.Content = view;
            }
            this.Tabs.Add(newtab);
            this.SelectedTabIndex = this.Tabs.IndexOf(newtab);
        }
        #endregion


        #region CurrentProject変更通知プロパティ
        private Project _CurrentProject;

        public Project CurrentProject
        {
            get
            { return _CurrentProject; }
            set
            {
                if (_CurrentProject == value)
                    return;
                _CurrentProject = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Tabs変更通知プロパティ
        private ObservableCollection<TabItem> _Tabs;

        public ObservableCollection<TabItem> Tabs
        {
            get
            { return _Tabs; }
            set
            {
                if (_Tabs == value)
                    return;
                _Tabs = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsLoadedProject変更通知プロパティ
        private bool _IsLoadedProject;

        public bool IsLoadedProject
        {
            get
            { return _IsLoadedProject; }
            set
            {
                if (_IsLoadedProject == value)
                    return;
                _IsLoadedProject = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region TaskText変更通知プロパティ
        private string _TaskText;

        public string TaskText
        {
            get
            { return _TaskText; }
            set
            {
                if (_TaskText == value)
                    return;
                _TaskText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region RecentProjects変更通知プロパティ
        private ObservableCollection<Project> _RecentProjects;

        public ObservableCollection<Project> RecentProjects
        {
            get
            { return _RecentProjects; }
            set
            {
                if (_RecentProjects == value)
                    return;
                _RecentProjects = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedTabIndex変更通知プロパティ
        private int _SelectedTabIndex;

        public int SelectedTabIndex
        {
            get
            { return _SelectedTabIndex; }
            set
            {
                if (_SelectedTabIndex == value)
                    return;
                _SelectedTabIndex = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
